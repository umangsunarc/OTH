using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wollo.Entities.Models;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Model = Wollo.Web.Models;
using System.Collections;
using Wollo.Entities.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Wollo.Web.Helper;
using Wollo.Web.Models;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;
using System.Net.Mail;
using System.Net;
using Wollo.Web.Common;

namespace Wollo.Web.Controllers
{
    [Authorize]
    public class TradingController : BaseController
    {
        /// <summary>
        /// Send email from application
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string sendMail(Wollo.Entities.Models.User user, string Subject, string Message)
        {
            try
            {
                Task<string>[] taskArray = { 
                                     Task<string>.Factory.StartNew(() => Mail.sendMail(user, Subject, Message))};
                return "success";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }

            //string htmlMessage;
            //htmlMessage = Resource.EmailTemplate.ToString();
            //htmlMessage = htmlMessage.Replace("&gt;", ">");
            //htmlMessage = htmlMessage.Replace("&lt;", "<");
            //htmlMessage = htmlMessage.Replace("\r", "");
            //htmlMessage = htmlMessage.Replace("\n", "");
            //htmlMessage = htmlMessage.Replace("\t", "");
            //htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
            //using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
            //{
            //    m.To.Add(new System.Net.Mail.MailAddress(user.email_address));
            //    m.Subject = Subject;
            //    m.Body = string.Format(htmlMessage,
            //    user.user_name + " ", Message, DateTime.UtcNow.Year);
            //    m.IsBodyHtml = true;
            //    try
            //    {
            //        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
            //        {
            //            smtp.Send(m);
            //            return "success";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        string message = ex.InnerException.Message.ToString();
            //        return message;
            //    }

            //}
            //return "fail";
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Wollo.Entities.Models.User> GetUserById(string userId)
        {
            Wollo.Entities.Models.User user = new Wollo.Entities.Models.User();
            //string email = model.Email;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/GetUserById?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<Wollo.Entities.Models.User>(responseData);
            }
            return user;
        }


        [HttpGet]
        public async Task<ActionResult> Trading(int? stockId)
        {
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            string userId = User.Identity.GetUserId();

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

            Wollo.Entities.ViewModels.TradingViewModel tradingObj = new Wollo.Entities.ViewModels.TradingViewModel();
            tradingObj = await GetAllTransactionHistoryByUser(userId);

            Wollo.Entities.ViewModels.QueueDataViewModel queueObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            queueObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));

            tradingObj.Transaction_History = tradingObj.Transaction_History.Select(x => new Wollo.Entities.ViewModels.Transaction_History
            {
                id = x.id,
                original_order_quantity = x.original_order_quantity,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                reward_points = x.reward_points,
                user_id = x.user_id,
                stock_code_id = x.stock_code_id,
                StockCode = x.StockCode,
                price = x.price,
                rate = x.rate,
                queue_action = x.queue_action,
                QueueStatusMaster = x.QueueStatusMaster,
                status_id = x.status_id
            }).ToList();

            //queueObj.TradedHistory = queueObj.TradedHistory.Select(x => new Wollo.Entities.ViewModels.Traded_History_Master
            //{
            //    id = x.id,
            //    queue_action = x.queue_action,
            //    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
            //    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
            //    created_by = x.created_by,
            //    updated_by = x.updated_by,
            //    amount = x.amount,
            //    bid_price = x.bid_price,
            //    ask_price = x.ask_price,
            //    StockCode = x.StockCode,
            //    stock = x.stock
            //}).ToList();

            //tradingObj.queue_data = await GetLatestBidAsk(Convert.ToInt32(stockId));

            Wollo.Entities.ViewModels.QueueTradingViewModel queueTradingObj = new Wollo.Entities.ViewModels.QueueTradingViewModel();
            queueTradingObj.Bid = queueObj.Bid;
            queueTradingObj.Ask = queueObj.Ask;
            queueTradingObj.TradedHistory = queueObj.TradedHistory;
            queueTradingObj.QueueData = queueObj.QueueData;
            queueTradingObj.Stock_Code = tradingObj.Stock_Code;
            queueTradingObj.unit_master = queueObj.UnitMaster;
            queueTradingObj.Transaction_History = tradingObj.Transaction_History;

            ViewBag.StockCode = tradingObj.Stock_Code;
            ViewBag.StockId = stockId;
            ViewBag.user_id = userId;
            ViewBag.UserName = User.Identity.Name;
            //return View(resultObj.Transaction_History);
            Model.QueueTradingModel model = new QueueTradingModel();
            model.QueueTradingViewModel = queueTradingObj;
            return View(model);
        }

        public async Task<TradingViewModel> GetAllTransactionHistoryByUser(string userId)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetAllTransactionHistorByUserUrl = domain + "api/Trading/GetAllTransactionHistoryByUser?userId=" + userId;
            client.BaseAddress = new Uri(GetAllTransactionHistorByUserUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetAllTransactionHistorByUserUrl);
            Wollo.Entities.ViewModels.TradingViewModel resultObj = null;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.TradingViewModel>(responseData);
                resultObj.Stock_Code = resultObj.Stock_Code.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                resultObj.Transaction_History = resultObj.Transaction_History.Where(x => x.user_id == userId).ToList();
                foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.Stock_Code)
                {
                    code.full_name = code.stock_name;
                }
            }
            return resultObj;
        }

        public async Task<QueueDataViewModel> GetTransactionHistoryByStock(int stockId)
        {

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
            }
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return resultObj;
        }

        public async Task<Models.TradingTimeDetailsViewModel> GetCurrentTradingTimeSettings()
        {
            string userId = User.Identity.GetUserId();
            Models.TradingTimeDetailsViewModel result = new Models.TradingTimeDetailsViewModel();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/AdminSetting/GetCurrentTradingTimeDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<Models.TradingTimeDetailsViewModel>(responseData);
            }
            //List<WeakDays> weakDays = await GetWeakDays();
            //List<int> selectedDays = new List<int>();
            //result.Days = weakDays;
            //ViewBag.UserName = User.Identity.Name;
            return result;
        }

        public async Task<JsonResult> CreateMarketRateGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            Models.TradingTimeDetailsViewModel timeDetails = new Models.TradingTimeDetailsViewModel();
            timeDetails = await GetCurrentTradingTimeSettings();
            if (timeDetails.start_time == null)
            {
                timeDetails.start_time = DateTime.Now.TimeOfDay;
            }
            if (timeDetails.end_time == null)
            {
                timeDetails.end_time = DateTime.Now.TimeOfDay;
            }
            DateTime min_time = new DateTime(timeDetails.start_time.Value.Ticks);
            DateTime max_time = new DateTime(timeDetails.end_time.Value.Ticks);
            //string min = GetTime(min_time).ToString();
            //string max = GetTime(max_time).ToString();

            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));

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
            //Models.TradingTimeDetailsViewModel timeDetails = new TradingTimeDetailsViewModel();
            //timeDetails = await GetCurrentTradingTimeSettings();
            if (duration == 1)
            {
                DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }
            else if (duration == 0)
            {
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.id != 0).ToList();
            }
            else if (duration == 30)
            {
                DateTime startDateTime = DateTime.Today.AddMonths(-1); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today; //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }
            else if (duration == 180)
            {
                DateTime startDateTime = DateTime.Today.AddMonths(-6); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today; //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }
            else if (duration == 365)
            {
                DateTime startDateTime = DateTime.Today.AddYears(-1); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today; //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }
            else if (duration == 1826)
            {
                DateTime startDateTime = DateTime.Today.AddYears(-5); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today; //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }
            else
            {
                DateTime startDateTime = DateTime.Today.AddDays(-(duration) + 1); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today; //Today at 23:59:59
                resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();
            }

            //**************************************** Code for trading graph************************************************//
            DateTime? lastTradedTime = resultObj.TradedHistory.Select(x => x.updated_date).Max();
            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date)
            .Select(n => new
            {
                Date = GetDateTime(n.FirstOrDefault().updated_date),
                Close = n.FirstOrDefault().updated_date == lastTradedTime ? (resultObj.TradedHistory.FirstOrDefault().bid_price).ToString() : "-",
                Open = resultObj.TradedHistory.LastOrDefault().bid_price,
                Low = resultObj.TradedHistory.Where(z => z.updated_date <= n.FirstOrDefault().updated_date).Select(p => p.bid_price).Min(),
                High = resultObj.TradedHistory.Where(z => z.updated_date <= n.FirstOrDefault().updated_date).Select(p => p.bid_price).Max(),
                Volume = resultObj.TradedHistory.Where(z => z.updated_date == n.FirstOrDefault().updated_date).Select(x => x.amount).Sum()
            }
            );
            int count = resultObj.TradedHistory.Count;
            string[,] CellArr1 = new string[count, 2];
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

            //*************************************** Code for volume graph ************************************************//
            int groupCount = resultObj.TradedHistory.Count;
            string[,] volumeArr1 = new string[groupCount, 2];
            DateTime v_date = new DateTime();
            for (int i = 1; i <= groupCount; i++)
            {
                DateTime tmp;
                if (DateTime.TryParse(resultObj.TradedHistory[i - 1].updated_date.ToString(), out tmp))
                {
                    v_date = tmp;
                }
                else
                {
                    v_date = DateTime.UtcNow;
                }
                string xdata = GetTime(v_date).ToString();
                string ydata = resultObj.TradedHistory[i - 1].amount.ToString();
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = ydata;
            }

            int m = 1;
            string[,][,] v_arr1 = new string[groupCount, m][,];
            for (int r = 0; r < groupCount; r++)
            {
                string[,] a = new string[1, 2];
                a[0, 0] = volumeArr1[r, 0];
                a[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = a;
            }
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            return Json(new { data1 = r1, data2 = r2, data3 = min_time, data4 = max_time, data5 = r3 }, JsonRequestBehavior.AllowGet);
            //var groups = resultObj.TradedHistory.GroupBy(n => n.updated_date)
            //.Select(n => new
            //{
            //    Date = n.Key,
            //    //Count = n.Count()
            //    Count=n.Sum(x=>x.amount)
            //}
            //)
            //.OrderBy(n => n.Date).ToList();
            //int groupCount = groups.Count();
            //string[,] volumeArr1 = new string[groupCount, 2];
            ////string[] x_axis = new string[groupCount];
            ////string[] y_axis = new string[groupCount];
            //DateTime v_date = new DateTime();
            //for (int i = 1; i <= groupCount; i++)
            //{
            //    DateTime tmp;
            //    if (DateTime.TryParse(groups[i - 1].Date.ToString(), out tmp))
            //    {
            //        v_date = tmp;
            //    }
            //    else
            //    {
            //        v_date = DateTime.UtcNow;
            //    }
            //    string xdata = GetTime(v_date).ToString();
            //    string ydata = groups[i - 1].Count.ToString();
            //    volumeArr1[i - 1, 0] = xdata;
            //    volumeArr1[i - 1, 1] = ydata;
            //}

            //int m = 1;
            //string[,][,] v_arr1 = new string[groupCount, m][,];
            //for (int r = 0; r < groupCount; r++)
            //{
            //    string[,] a = new string[1, 2];
            //    a[0, 0] = volumeArr1[r, 0];
            //    a[0, 1] = volumeArr1[r, 1];
            //    v_arr1[r, 0] = a;
            //}

        }

        public async Task<JsonResult> CreateFiveDayGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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

            DateTime startDateTime = DateTime.Today.AddDays(-(duration) + 1); //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); ; //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);
            //*************************************** Code for volume graph ************************************************//
            //var groups = resultObj.TradedHistory.GroupBy(n => n.updated_date)
            //.Select(n => new
            //{
            //    Date = n.Key,
            //    //Count = n.Count()
            //    Count = n.Sum(x => x.amount)
            //}
            //)
            //.OrderBy(n => n.Date).ToList();
            //int groupCount = groups.Count();
            //string[,] volumeArr1 = new string[groupCount, 2];
            //DateTime v_date = new DateTime();
            //for (int i = 1; i <= groupCount; i++)
            //{
            //    DateTime tmp;
            //    if (DateTime.TryParse(groups[i - 1].Date.ToString(), out tmp))
            //    {
            //        v_date = tmp;
            //    }
            //    else
            //    {
            //        v_date = DateTime.UtcNow;
            //    }
            //    string xdata = GetTime(v_date).ToString();
            //    string ydata = groups[i - 1].Count.ToString();
            //    volumeArr1[i - 1, 0] = xdata;
            //    volumeArr1[i - 1, 1] = ydata;
            //}

            //int m = 1;
            //string[,][,] v_arr1 = new string[groupCount, m][,];
            //for (int r = 0; r < groupCount; r++)
            //{
            //    string[,] a = new string[1, 2];
            //    a[0, 0] = volumeArr1[r, 0];
            //    a[0, 1] = volumeArr1[r, 1];
            //    v_arr1[r, 0] = a;
            //}
        }

        public async Task<JsonResult> CreateOneMonthGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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

            DateTime startDateTime = DateTime.Today.AddMonths(-1); //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); ; //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> CreateSixMonthGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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
            DateTime startDateTime = DateTime.Today.AddMonths(-6); //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> CreateOneYearGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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
            DateTime startDateTime = DateTime.Today.AddYears(-1); //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> CreateFiveYearGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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
            DateTime startDateTime = DateTime.Today.AddYears(-5); //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.updated_date >= startDateTime && x.updated_date <= endDateTime).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> CreateFullGraph(int? stockId, int duration)
        {
            string userId = User.Identity.GetUserId();
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            Wollo.Entities.ViewModels.QueueDataViewModel resultObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            resultObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));
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
            resultObj.TradedHistory = resultObj.TradedHistory.Where(x => x.id != 0).ToList();

            var dayGroups = resultObj.TradedHistory.GroupBy(n => n.updated_date.Value.Date)
            .Select(n => new
            {
                Date = GetTime(n.Key),
                Close = n.FirstOrDefault().bid_price,
                Open = n.LastOrDefault().bid_price,
                Low = n.Select(x => x.bid_price).Min(),
                High = n.Select(x => x.bid_price).Max(),
                Volume = n.Select(x => x.amount).Sum()
            }
            )
            .OrderBy(n => n.Date).ToList();
            int count = dayGroups.Count;
            string[,] CellArr1 = new string[count, 2];
            string[,] volumeArr1 = new string[count, 2];
            for (int i = 1; i <= count; i++)
            {
                string xdata = dayGroups[i - 1].Date.ToString();
                string ydata = dayGroups[i - 1].Close.ToString();
                string volume_ydata = dayGroups[i - 1].Volume.ToString();
                CellArr1[i - 1, 0] = xdata;
                CellArr1[i - 1, 1] = ydata;
                volumeArr1[i - 1, 0] = xdata;
                volumeArr1[i - 1, 1] = volume_ydata;
            }

            int k = 1;
            string[,][,] arr1 = new string[count, k][,];
            string[,][,] v_arr1 = new string[count, k][,];
            for (int r = 0; r < count; r++)
            {
                //*********************** Price chart data**************************//
                string[,] a = new string[1, 2];
                a[0, 0] = CellArr1[r, 0];
                a[0, 1] = CellArr1[r, 1];
                arr1[r, 0] = a;

                //**********************Volume chart data***************************//
                string[,] b = new string[1, 2];
                b[0, 0] = volumeArr1[r, 0];
                b[0, 1] = volumeArr1[r, 1];
                v_arr1[r, 0] = b;
            }
            var r1 = JsonConvert.SerializeObject(CellArr1);
            var r3 = JsonConvert.SerializeObject(dayGroups);
            var r2 = JsonConvert.SerializeObject(volumeArr1);
            return Json(new { data1 = r1, data2 = r2, data3 = r3 }, JsonRequestBehavior.AllowGet);

        }



        public static long GetTime(DateTime input)
        {
            System.TimeSpan span = new System.TimeSpan(System.DateTime.Parse("1/1/1970").Ticks);
            System.DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }

        public static long GetDateTime(DateTime? input)
        {
            DateTime date = new DateTime();
            DateTime tmp;
            if (DateTime.TryParse(input.ToString(), out tmp))
            {
                date = tmp;
            }
            System.TimeSpan span = new System.TimeSpan(System.DateTime.Parse("1/1/1970").Ticks);
            System.DateTime time = date.Subtract(span);
            return (long)(time.Ticks / 10000);
        }

        [HttpGet]
        [AuthorizeRole(Module = "Trading", Permission = "View")]
        public async Task<QueueData> GetLatestBidAsk(int StockId)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetLatestBidAskByStockUrl = domain + "api/Trading/GetLatestBidAskByStock?StockId=" + StockId;
            client.BaseAddress = new Uri(GetLatestBidAskByStockUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetLatestBidAskByStockUrl);
            Wollo.Entities.ViewModels.QueueData latesBidAsk = new Wollo.Entities.ViewModels.QueueData();
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                latesBidAsk = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.QueueData>(responseData);
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
            ViewBag.UserName = User.Identity.Name;
            return latesBidAsk;
        }

        [AuthorizeRole(Module = "Trading", Permission = "View")]
        public async Task<ActionResult> GetLatestBidAskByStock(int StockId)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetLatestBidAskByStockUrl = domain + "api/Trading/GetLatestBidAskByStock?StockId=" + StockId;
            client.BaseAddress = new Uri(GetLatestBidAskByStockUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetLatestBidAskByStockUrl);
            Wollo.Entities.ViewModels.QueueData latesBidAsk = null;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                latesBidAsk = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.QueueData>(responseData);
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
            ViewBag.UserName = User.Identity.Name;
            return Json(latesBidAsk, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Module = "All Trading History", Permission = "View")]
        public async Task<ActionResult> AdminIndex()
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetAllTransactionHistory = domain + "api/Trading/GetAllTransactionHistory";
            client.BaseAddress = new Uri(GetAllTransactionHistory);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetAllTransactionHistory);
            List<Wollo.Entities.ViewModels.Transaction_History> lstTransactionHistory = null;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                lstTransactionHistory = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Transaction_History>>(responseData);
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
            else
            {
            }
            ViewBag.UserName = User.Identity.Name;
            return View(lstTransactionHistory);
        }

        [AuthorizeRole(Module = "Trading", Permission = "Create")]
        public async Task<ActionResult> AddBidAsk(int point, float price, string stock_code_id, string task)
        {
            string userId = User.Identity.GetUserId();
            Wollo.Entities.ViewModels.Transaction_History transactionHistory = new Wollo.Entities.ViewModels.Transaction_History();
            transactionHistory.reward_points = point;
            transactionHistory.original_order_quantity = point;
            transactionHistory.price = price;
            transactionHistory.queue_action = task;
            transactionHistory.user_id = User.Identity.GetUserId();
            transactionHistory.stock_code_id = Convert.ToInt32(stock_code_id);
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Trading/AddBidAsk";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, transactionHistory);
            var responseData = "";
            if (responseMessage.IsSuccessStatusCode)
            {
                responseData = responseMessage.Content.ReadAsStringAsync().Result;
                responseData = (JsonConvert.DeserializeObject<int>(responseData)).ToString();
                if (responseData == "1")
                {
                    Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                    objuser = await GetUserById(userId);
                    string re = sendMail(objuser, " Bid/Ask Success!", "<div><p>You have traded an amount of "+point+" shares at  $"+price+". Please check your transaction history for more information. <br/>Thank you! </p></div>");
                }
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
            else
            {
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Module = "Trading", Permission = "Update")]
        public async Task<ActionResult> UpdateTransaction(int id, int point, float price)
        {
            string userId = User.Identity.GetUserId();
            Wollo.Entities.ViewModels.Transaction_History transactionHistory = new Wollo.Entities.ViewModels.Transaction_History();
            transactionHistory.reward_points = point;
            transactionHistory.price = price;
            transactionHistory.id = id;
            transactionHistory.user_id = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Trading/UpdateTransaction";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, transactionHistory);
            var responseData = "";
            if (responseMessage.IsSuccessStatusCode)
            {
                responseData = responseMessage.Content.ReadAsStringAsync().Result;
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
            else
            {
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Module = "Trading", Permission = "Update")]
        public async Task<ActionResult> CancelTransaction(int transactionid)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Trading/CancelTransaction?Id=" + transactionid + "&userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            var responseData = "";
            if (responseMessage.IsSuccessStatusCode)
            {
                responseData = responseMessage.Content.ReadAsStringAsync().Result;
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
            else
            {
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Change Language
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ChangeCurrentCulture(int id)
        {
            await Task.Yield();
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;
            //  
            // Redirect to the same page from where the request was made!   
            //  
            return Redirect(Request.UrlReferrer.ToString());
        }

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

        //[HttpPost]
        //public async Task<ActionResult> GetAllQueueTradingDataByStock(int? stockId)
        //{
        //    //********************* For fisrt time the queue screen will show reward point queue by default***************//
        //    if (stockId == null)
        //    {
        //        stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
        //    }
        //    string userId = User.Identity.GetUserId();
        //    Wollo.Entities.ViewModels.Transaction_History history = new Wollo.Entities.ViewModels.Transaction_History();
        //    history.user_id = userId;
        //    history.stock_code_id = Convert.ToInt32(stockId);
        //    HttpClient client = new HttpClient();
        //    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
        //    string GetAllTransactionHistorByUserUrl = domain + "api/Queue/GetAllQueueTradingDataByStock";
        //    client.BaseAddress = new Uri(GetAllTransactionHistorByUserUrl);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage Message = await client.PostAsJsonAsync(GetAllTransactionHistorByUserUrl, history);
        //    Wollo.Entities.ViewModels.QueueTradingViewModel resultObj = new Wollo.Entities.ViewModels.QueueTradingViewModel();
        //    if (Message.IsSuccessStatusCode)
        //    {
        //        var responseData = Message.Content.ReadAsStringAsync().Result;
        //        resultObj = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.QueueTradingViewModel>(responseData);
        //        resultObj.Stock_Code = resultObj.Stock_Code.Where(x => x.stock_code.ToLower() != "issue points").ToList();
        //        foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.Stock_Code)
        //        {
        //            code.full_name = code.stock_code + "-" + code.stock_name;
        //        }
        //    }

        //    ViewBag.StockCode = resultObj.Stock_Code;
        //    ViewBag.StockId = stockId;
        //    ViewBag.user_id = userId;
        //    ViewBag.UserName = User.Identity.Name;
        //    //return View(resultObj.Transaction_History);
        //    return View(resultObj);
        //}

        //
        // GET: /Trading/
        [AuthorizeRole(Module = "Trading", Permission = "View")]
        public async Task<ActionResult> Index(int? StockId)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetAllTransactionHistorByUserUrl = domain + "api/Trading/GetAllTransactionHistoryByUser?userId=" + userId;
            client.BaseAddress = new Uri(GetAllTransactionHistorByUserUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetAllTransactionHistorByUserUrl);
            Wollo.Entities.ViewModels.TradingViewModel resultObj = null;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.TradingViewModel>(responseData);
                resultObj.Stock_Code = resultObj.Stock_Code.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                resultObj.Transaction_History = resultObj.Transaction_History.Where(x => x.user_id == userId).ToList();
                foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.Stock_Code)
                {
                    code.full_name = code.stock_name;
                }
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
            else
            {
            }
            if (StockId == null || StockId == 0)
            {
                StockId = resultObj.Stock_Code.FirstOrDefault().id;
            }
            resultObj.queue_data = await GetLatestBidAsk(Convert.ToInt32(StockId));
            ViewBag.StockCode = resultObj.Stock_Code;
            ViewBag.StockId = StockId;
            ViewBag.user_id = userId;
            ViewBag.UserName = User.Identity.Name;
            //return View(resultObj.Transaction_History);
            return View(resultObj);
        }
        public async Task<ActionResult> PortfolioDetails()
        {
            string userId = User.Identity.GetUserId();
            List<PortfolioDetails> resultObj = new List<PortfolioDetails>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Trading/GetPortfolioDetails?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<PortfolioDetails>>(responseData);

            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            Model.Portfolio model = new Model.Portfolio();
            model.PortfolioDetails = resultObj;
            return View(model);

        }

    }
}