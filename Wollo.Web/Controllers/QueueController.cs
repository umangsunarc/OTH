using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    //[Authorize]
    public class QueueController : BaseController
    {
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

        //
        // GET: /Queue/
        [AuthorizeRole(Module = "Queue", Permission = "View")]
        public async Task<ActionResult> Index(int? stockId)
        {
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            string userId = User.Identity.GetUserId();

            Wollo.Entities.ViewModels.TradingViewModel tradingObj = new Wollo.Entities.ViewModels.TradingViewModel();
            tradingObj = await GetAllTransactionHistoryByUser(userId);

            Wollo.Entities.ViewModels.QueueDataViewModel queueObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            queueObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));

            tradingObj.queue_data = await GetLatestBidAsk(Convert.ToInt32(stockId));

            Wollo.Entities.ViewModels.QueueTradingViewModel queueTradingObj = new Wollo.Entities.ViewModels.QueueTradingViewModel();
            queueTradingObj.Bid = queueObj.Bid;
            queueTradingObj.Ask = queueObj.Ask;
            queueTradingObj.TradedHistory = queueObj.TradedHistory;
            queueTradingObj.QueueData = queueObj.QueueData;
            queueTradingObj.Stock_Code = tradingObj.Stock_Code;
            queueTradingObj.unit_master = tradingObj.unit_master;
            queueTradingObj.Transaction_History = tradingObj.Transaction_History;
            //*******************************Code to save audit log details start***************************//
            Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
            logDetails.user_id = userId;
            logDetails.created_date = DateTime.UtcNow;
            logDetails.updated_date = DateTime.UtcNow;
            logDetails.created_by = userId;
            logDetails.updated_by = userId;
            logDetails.url = HttpContext.Request.Url.AbsoluteUri;
            int r = await SaveAuditLogDetails(logDetails);
            //*******************************Code to save audit log details start***************************//
            queueTradingObj.TradedHistory = queueTradingObj.TradedHistory.Select(x => new Wollo.Entities.ViewModels.Traded_History_Master
            {
                id = x.id,
                queue_action = x.queue_action,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                amount = x.amount,
                bid_price = x.bid_price,
                ask_price = x.ask_price,
                StockCode = x.StockCode,
                stock = x.stock
            }).ToList();
            ViewBag.StockCode = tradingObj.Stock_Code;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return View(queueTradingObj);
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
                    code.full_name = code.stock_code + "-" + code.stock_name;
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
            resultObj.TradedHistory = resultObj.TradedHistory.Select(x => new Wollo.Entities.ViewModels.Traded_History_Master
            {
                id = x.id,
                queue_action = x.queue_action,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                amount = x.amount,
                bid_price = x.bid_price,
                ask_price = x.ask_price,
                StockCode = x.StockCode,
                stock = x.stock
            }).ToList();
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return resultObj;
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
            return latesBidAsk;
        }

        //
        // GET: /Queue/
        [AuthorizeRole(Module = "Queue", Permission = "View")]
        public async Task<ActionResult> Index1(int? stockId)
        {
            string userId = User.Identity.GetUserId();
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
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
            var result = "";
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
                resultObj.StockCode = resultObj.StockCode.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int r = await SaveAuditLogDetails(logDetails);
                //*******************************Code to save audit log details start***************************//
            }
            else
            {
                ViewBag.UserName = result.ToString();
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.StockCode)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            resultObj.TradedHistory = resultObj.TradedHistory.Select(x => new Wollo.Entities.ViewModels.Traded_History_Master
            {
                id = x.id,
                queue_action = x.queue_action,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                amount = x.amount,
                bid_price = x.bid_price,
                ask_price = x.ask_price,
                StockCode = x.StockCode,
                stock = x.stock
            }).ToList();
            ViewBag.StockCode = resultObj.StockCode;
            ViewBag.StockId = stockId.ToString();
            ViewBag.UserName = User.Identity.Name;
            return View(resultObj);
        }

        public async Task<ActionResult> MatchQueue(int stockId, string userId)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/MatchQueue?stockId=" + stockId + "&userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            //var result = "";
            string[] result=new string[2];
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<string[]>(responseData);
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int r = await SaveAuditLogDetails(logDetails);
                //*******************************Code to save audit log details start***************************//
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result[0] = "Call to action Failed";
                result[1] = "fail";
                ViewBag.UserName = result.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}