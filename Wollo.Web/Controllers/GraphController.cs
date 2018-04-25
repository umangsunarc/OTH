using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wollo.Entities.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Web.Controllers.Helper;


namespace Wollo.Web.Controllers
{
    public class GraphController : Controller
    {
        /// <summary>
        /// Save audit log details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SaveAuditLogDetails(Wollo.Entities.Models.Audit_Log_Master model)
        {
            model.ip_address = GetIPAddress.GetVisitorIPAddress();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Logs/SaveAuditLogDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, model);
            int result = 0;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
            }
            return result;
        }

        //
        // GET: /Graph/
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CreateChartByStock(int? stockId)
        {
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            QueueDataViewModel resultObj = new QueueDataViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
                //resultObj.StockCode = resultObj.StockCode.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            //foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.StockCode)
            //{
            //    code.full_name = code.stock_code + "-" + code.stock_name;
            //}
            //ViewBag.StockCode = resultObj.StockCode;
            //ViewBag.StockId = stockId.ToString();
            //ViewBag.UserName = User.Identity.Name;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            //resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            var groups = resultObj.TradedHistory.GroupBy(n => n.updated_date)
            .Select(n => new
            {
                Date = n.Key,
                Count = n.Count()
            }
            )
            .OrderBy(n => n.Date).ToList();
            //int count = resultObj.TradedHistory.Count;
            int count = groups.Count();
            string[,] CellArr1 = new string[count, 2];
            string[] xaxis = new string[count];
            string[] yaxis = new string[count];
            DateTime date = new DateTime();
            for (int i = 1; i <= count; i++)
            {
                DateTime tmp;
                if (DateTime.TryParse(groups[i - 1].Date.ToString(), out tmp))
                {
                    date = tmp;
                }
                else
                {
                    date = DateTime.UtcNow;
                }
                string xdata = GetTime(date).ToString();
                string ydata = groups[i - 1].Count.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            return Json(new { data = r1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> BidGraph(int? stockId)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Master/GetAllStockCode";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            List<Stock_Code> resultObj = new List<Stock_Code>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int result = await SaveAuditLogDetails(logDetails);
                //*******************************Code to save audit log details start***************************//
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public async Task<ActionResult> AskGraph(int? stockId)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Master/GetAllStockCode";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            List<Stock_Code> resultObj = new List<Stock_Code>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int result = await SaveAuditLogDetails(logDetails);
                //*******************************Code to save audit log details start***************************//
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public async Task<ActionResult> VolumeGraph(int? stockId)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Master/GetAllStockCode";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            List<Stock_Code> resultObj = new List<Stock_Code>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int result = await SaveAuditLogDetails(logDetails);
                //*******************************Code to save audit log details start***************************//
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public static long GetTime(DateTime input)
        {
            System.TimeSpan span = new System.TimeSpan(System.DateTime.Parse("1/1/1970").Ticks);
            System.DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }

        public async Task<JsonResult> CreateBidGraph(int? stockId)
        {
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            QueueDataViewModel resultObj = new QueueDataViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
                resultObj.StockCode = resultObj.StockCode.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.StockCode)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.queue_action.ToLower() == "bid" && x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            int count = resultObj.TradedHistory.Count;
            string[,] CellArr1 = new string[count, 2];
            string[] xaxis = new string[count];
            string[] yaxis = new string[count];
            DateTime date = new DateTime();
            for (int i = 1; i <= count; i++)
            {
                DateTime tmp;
                if (DateTime.TryParse(resultObj.TradedHistory[i - 1].updated_date.ToString(), out tmp))
                {
                    date = tmp;
                }
                else
                {
                    date = DateTime.UtcNow;
                }
                string xdata = GetTime(date).ToString();
                string ydata = resultObj.TradedHistory[i - 1].bid_price.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            return Json(new { data = r1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateAskGraph(int? stockId)
        {
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            QueueDataViewModel resultObj = new QueueDataViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
                resultObj.StockCode = resultObj.StockCode.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.StockCode)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.queue_action.ToLower() == "ask" && x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            int count = resultObj.TradedHistory.Count;
            string[,] CellArr1 = new string[count, 2];
            string[] xaxis = new string[count];
            string[] yaxis = new string[count];
            DateTime date = new DateTime();
            for (int i = 1; i <= count; i++)
            {
                DateTime tmp;
                if (DateTime.TryParse(resultObj.TradedHistory[i - 1].updated_date.ToString(), out tmp))
                {
                    date = tmp;
                }
                else
                {
                    date = DateTime.UtcNow;
                }
                string xdata = GetTime(date).ToString();
                string ydata = resultObj.TradedHistory[i - 1].bid_price.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            return Json(new { data = r1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateVolumeGraph(int? stockId)
        {
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            QueueDataViewModel resultObj = new QueueDataViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
                resultObj.StockCode = resultObj.StockCode.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.StockCode)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            var groups = resultObj.TradedHistory.GroupBy(n => n.updated_date)
            .Select(n => new
            {
                Date = n.Key,
                Count = n.Count()
            }
            )
            .OrderBy(n => n.Date).ToList();
            //int count = resultObj.TradedHistory.Count;
            int count = groups.Count();
            string[,] CellArr1 = new string[count, 2];
            string[] xaxis = new string[count];
            string[] yaxis = new string[count];
            DateTime date = new DateTime();
            for (int i = 1; i <= count; i++)
            {
                DateTime tmp;
                if (DateTime.TryParse(groups[i - 1].Date.ToString(), out tmp))
                {
                    date = tmp;
                }
                else
                {
                    date = DateTime.UtcNow;
                }
                string xdata = GetTime(date).ToString();
                string ydata = groups[i - 1].Count.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            return Json(new { data = r1 }, JsonRequestBehavior.AllowGet);
        }
    }
}