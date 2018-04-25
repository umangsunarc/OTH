using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Web.Controllers.Helper;
using Wollo.Web.Models;
using Wollo.Entities.ViewModels;
using Wollo.Web.Common;
using Wollo.Base.LocalResource;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    public class TransferRewardPointsController : BaseController
    {
        public TransferRewardPointsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public TransferRewardPointsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            //userManager.UserValidator=
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

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

        /// <summary>
        /// Send Email from application
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
        }

        //for filtering in a date range
        [HttpGet]
        public async Task<ActionResult> RangeFilter(DateTime start_date, DateTime end_date)
        {
            Model.RewardPointsTransferDetailsModel model = new RewardPointsTransferDetailsModel();
            Reward_Points_Transfer_Master master = new Reward_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> result = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/TransferRewardPoints/RangeFilter";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // HttpResponseMessage Message = await client.GetAsync(url, master);
            HttpResponseMessage Message = await client.PostAsJsonAsync(url, master);

            if (Message.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(responseData);
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
                model.RewardPointsTransferDetails = result;
                return View("AdminIndex", model);
            }
            else
            {
                return View("AdminIndex", model);
            }
        }

        public async Task<ActionResult> RangeFilter1(DateTime start_date, DateTime end_date, int? stockId)
        {
            string userId = User.Identity.GetUserId();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            TempData["StartDate"] = start_date.Date;
            TempData["EndDate"] = end_date.Date;
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }

            Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            objuser = await GetUserById(userId);

            Wollo.Entities.ViewModels.Member_Stock_Details objStockDetails = new Entities.ViewModels.Member_Stock_Details();
            objStockDetails.user_id = objuser.user_id;
            objStockDetails.stock_code_id = Convert.ToInt32(stockId);
            objStockDetails = await GetStockDetailsByUserAndStock(objStockDetails);
            objStockDetails.email = objuser.email_address;

            List<Wollo.Entities.ViewModels.Stock_Code> objStock = new List<Entities.ViewModels.Stock_Code>();
            objStock = await GetAllStocks();

            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> pointTransferDetails = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            pointTransferDetails = await GetAllPointsTransferHistory();

            if (master.created_date == master.updated_date)
            {
                pointTransferDetails = pointTransferDetails.Where(x => x.RewardPointTransferMaster.created_date.Value.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                pointTransferDetails = pointTransferDetails.Where(x => x.RewardPointTransferMaster.created_date.Value.Date >= master.created_date.Value.Date && x.RewardPointTransferMaster.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

            }
            
            TransferPointsViewModel model = new TransferPointsViewModel();
            model.RewardPointsTransferDetails = pointTransferDetails;
            model.stock_amount = objStockDetails.stock_amount;
            model.stock_id = objStockDetails.stock_code_id;
            model.email = objStockDetails.email;

            ViewBag.StockCode = objStock;
            ViewBag.StockId = stockId;
            ViewBag.user_id = userId;
            ViewBag.UserName = User.Identity.Name;
            return View("Index",model);
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

        /// <summary>
        /// Get Member stock details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Wollo.Entities.ViewModels.Member_Stock_Details> GetStockDetailsByUserAndStock(Wollo.Entities.ViewModels.Member_Stock_Details details)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Stock/GetStockDetailsByUserAndStock";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.PostAsJsonAsync(url, details);

            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.Member_Stock_Details>(responseData);
            }
            return details;
        }


        [HttpGet]
        public async Task<ActionResult> AdminIndex()
        {
            Model.RewardPointsTransferDetailsModel model = new RewardPointsTransferDetailsModel();
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> result = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/TransferRewardPoints/GetAllPointsTransferHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(responseData);
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
                model.RewardPointsTransferDetails = result;
                return View(model);
            }
            else
            {
                return View(model);
            }

        }


        //
        // GET: /TransferRewardPoints/
        public async Task<ActionResult> Index(int? stockId)
        {
            string userId = User.Identity.GetUserId();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }

            Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            objuser = await GetUserById(userId);

            Wollo.Entities.ViewModels.Member_Stock_Details objStockDetails = new Entities.ViewModels.Member_Stock_Details();
            objStockDetails.user_id = objuser.user_id;
            objStockDetails.stock_code_id = Convert.ToInt32(stockId);
            objStockDetails = await GetStockDetailsByUserAndStock(objStockDetails);
            objStockDetails.email = objuser.email_address;

            List<Wollo.Entities.ViewModels.Stock_Code> objStock = new List<Entities.ViewModels.Stock_Code>();
            objStock = await GetAllStocks();

            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> pointTransferDetails = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            pointTransferDetails = await GetAllPointsTransferHistory();

            TransferPointsViewModel model = new TransferPointsViewModel();
            model.RewardPointsTransferDetails = pointTransferDetails;
            model.stock_amount = objStockDetails.stock_amount;
            model.stock_id = objStockDetails.stock_code_id;
            model.email = objStockDetails.email;

            //    //*******************************Code to save audit log details start***************************//
            Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
            logDetails.user_id = userId;
            logDetails.created_date = DateTime.UtcNow;
            logDetails.updated_date = DateTime.UtcNow;
            logDetails.created_by = userId;
            logDetails.updated_by = userId;
            logDetails.url = HttpContext.Request.Url.AbsoluteUri;
            int r = await SaveAuditLogDetails(logDetails);
            //    //*******************************Code to save audit log details start***************************//
            ViewBag.StockCode = objStock;
            ViewBag.StockId = stockId;
            ViewBag.user_id = userId;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        /// <summary>
        /// Transfer reward points from rpe to wollo
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(Wollo.Web.Models.TransferPointsViewModel data)
        {
            if (data.transfer_amount <= 0)
            {
                TempData["Fail"] = "The transfer amount should be greater than 0";
                return RedirectToAction("Index", new { stockId = data.stock_id });
            }
            if (data.stock_amount > data.transfer_amount)
            {
                Wollo.Entities.ViewModels.Stock_Code objStockCode = await GetStockCodeById(data.stock_id);
                Wollo.Web.Models.PointTransferModel model = new Models.PointTransferModel();
                model.email = data.email;
                model.stock_amount = data.transfer_amount;
                model.store_code = objStockCode.stock_code;
                string userId = User.Identity.GetUserId();
                Wollo.Entities.ViewModels.Result result = new Wollo.Entities.ViewModels.Result();
                HttpClient client = new HttpClient();
                //string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                //string url = domain + "api/TransferRewardPoin/GetCurrentMarketRateDetails";
                string domain = "http://www.wollo.co";
                string url = domain + "/transfer/reward_points.php";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.PostAsJsonAsync(url, model);
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.Result>(responseData);
                    //*******************************Code to save audit log details start***************************//
                    Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                    logDetails.user_id = userId;
                    logDetails.created_date = DateTime.UtcNow;
                    logDetails.updated_date = DateTime.UtcNow;
                    logDetails.created_by = userId;
                    logDetails.updated_by = userId;
                    logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                    int r = await SaveAuditLogDetails(logDetails);
                    if (result.status == 0)
                    {
                        //ViewBag.Message = "User with email address " + model.email + " does not exist.";
                        //TempData["Fail"] = "User with email address " + model.email + " does not exist.";
                        ViewBag.Message = result.msg;
                        TempData["Fail"] = result.msg;                           
                    }
                    else
                    {
                        string response = await TransferPointsFromRPEToWollo(model);
                        if (response == "success")
                        {
                            TempData["Success"] = result.msg;
                            ViewBag.Message = result.msg;
                        }
                        else if (response == "User does not exist.")
                        {
                            TempData["Fail"] = "User does not exist.";
                            ViewBag.Message = "User does not exist.";
                        }
                        else
                        {
                            ViewBag.Message = response;
                        }
                    }

                    //*******************************Code to save audit log details start***************************//
                }
                ViewBag.UserName = User.Identity.Name;
                return RedirectToAction("Index", new { stockId = data.stock_id });
            }
            else
            {
                TempData["Fail"] = "Sorry!Stock amount:" + data.stock_amount + " is less than transfer amount:" + data.transfer_amount + ".";
                return RedirectToAction("Index", new { stockId = data.stock_id });
            }

        }

        [HttpGet]
        public async Task<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>> GetAllPointsTransferHistory()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> result = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/TransferRewardPoints/GetAllPointsTransferHistoryByUser?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(responseData);
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
                return result;
            }
            else
            {
                return result;
            }

        }

        /// <summary>
        /// Update member stock details when points from rpe to wollo tranfer is successfull
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TransferPointsFromRPEToWollo(Wollo.Web.Models.PointTransferModel model)
        {
            string userId = User.Identity.GetUserId();
            Wollo.Entities.Models.Member_Stock_Details details = new Entities.Models.Member_Stock_Details();
            details.StockCode=new Wollo.Entities.Models.Stock_Code();
            details.user_id = userId;
            details.stock_amount = model.stock_amount;
            details.email = model.email;
            details.StockCode.stock_code = model.store_code;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/TransferRewardPoints/TransferPointsFromRPEToWollo";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.PostAsJsonAsync(url, details);
            string result = "";
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<string>(responseData);
                //*******************************Code to save audit log details start***************************//
                Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                logDetails.user_id = userId;
                logDetails.created_date = DateTime.UtcNow;
                logDetails.updated_date = DateTime.UtcNow;
                logDetails.created_by = userId;
                logDetails.updated_by = userId;
                logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                int r = await SaveAuditLogDetails(logDetails);
                Wollo.Entities.Models.User objUser = await GetUserById(userId);
                string re = sendMail(objUser, "Reward Points Transfer- Success!", "<div><p>You have successfully transferred "+model.stock_amount+" rewards points to your Wollo account. <br/>Thank you!</p></div>");
                //*******************************Code to save audit log details start***************************//
                return result;
            }
            else
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<string>(responseData);
                return result;
            }

        }

        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <returns></returns>
        public async Task<List<Wollo.Entities.ViewModels.Stock_Code>> GetAllStocks()
        {
            List<Wollo.Entities.ViewModels.Stock_Code> resultObj = new List<Wollo.Entities.ViewModels.Stock_Code>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Stock/GetStocks";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            else
            {
                // Api failed
            }
            foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj)
            {
                code.full_name = code.stock_code + "-" + code.stock_name;
            }
            return resultObj;
        }

        public async Task<Wollo.Entities.ViewModels.Stock_Code> GetStockCodeById(int id)
        {
            Wollo.Entities.ViewModels.Stock_Code resultObj = new Wollo.Entities.ViewModels.Stock_Code();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Stock/GetStockCodeById?id=" + id;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.Stock_Code>(responseData);
                return resultObj;
            }
            else
            {
                // Api failed
                return resultObj;
            }

        }

    }
}