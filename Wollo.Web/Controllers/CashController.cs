using Wollo.Entities.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Wollo.Web.Helper;
using System.Linq;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.Utilities;
using Wollo.Base.LocalResource;
using System.Net.Mail;
using System.Net;
using System.Web;
using Wollo.Web.Common;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    [Authorize]
    public class CashController : BaseController
    {
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
        /// for filtering cash withdrawal
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter(DateTime start_date, DateTime end_date)
        {
            Withdrawel_History_Master master = new Withdrawel_History_Master();
            WithdrawalData resultObj = new WithdrawalData();
            string userId = User.Identity.GetUserId();
            master.created_date = start_date;
            master.updated_date = end_date;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/RangeFilter";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<WithdrawalData>(responseData);
                }
                else
                {
                    // Api failed
                }
            }
            else
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/RangeFilterUser?userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<WithdrawalData>(responseData);
                }
                else
                {
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.Withdrawal model = new Model.Withdrawal();
            model.WithdrawalData = resultObj;
            return View("CashWithdr", model);
            //return Json(resultObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// for filtering cash transaction
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter1(DateTime start_date, DateTime end_date)
        {
            string userId = User.Identity.GetUserId();
            List<Cash_Transaction_History> lstCashTransactionHistory = new List<Cash_Transaction_History>();
            Cash_Transaction_History master = new Cash_Transaction_History();
            master.created_date = start_date;
            master.updated_date = end_date;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/RangeFilter1";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HttpResponseMessage responseMessage = await client.GetAsync(url);
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    lstCashTransactionHistory = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);


                }
                else
                {
                    // Api failed
                }

            }
            else
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/CashTransactionFilterUser?userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    lstCashTransactionHistory = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);

                }
                else
                {
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.CashTransactionModel model = new Model.CashTransactionModel();
            model.CashTransactionHistory = lstCashTransactionHistory;
            return View("CashTrans", model);

        }

        /// <summary>
        /// for filtering fund/topup in a date range
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter2(DateTime start_date, DateTime end_date)
        {
            string userId = User.Identity.GetUserId();
            TopUpDetails resultObj = new TopUpDetails();
            Topup_History master = new Topup_History();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            List<Wollo.Entities.ViewModels.Payment_Method> lstPaymentMethods = await GetAllPaymentMethods();
            ViewBag.PaymentMethodsList = lstPaymentMethods;
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilter2";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            //HttpResponseMessage responseMessage = await client.g(url,master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<TopUpDetails>(responseData);

            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            Model.FundTopup model = new Model.FundTopup();
            model.TopUpDetails = resultObj;
            return View("FundTopUp", model);
        }

        /// <summary>
        /// filter for request cash admin
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> Rangefilter3(DateTime start_date, DateTime end_date)
        {
            string userId = User.Identity.GetUserId();
            List<Issue_Cash_Transfer_Master> resultObj = new List<Issue_Cash_Transfer_Master>();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            //if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            //{
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilter3?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // HttpResponseMessage responseMessage = await client.GetAsync(url);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Cash_Transfer_Master>>(responseData);

            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = resultObj;
            return View("CashRequestAdmin", model);
        }
        //code ends here

        /// <summary>
        /// code for filtering issue cash
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter4(DateTime start_date, DateTime end_date)
        {
            Cash_Transaction_History master = new Cash_Transaction_History();
            List<Cash_Transaction_History> resultObj = new List<Cash_Transaction_History>();
            master.created_date = start_date;
            master.updated_date = end_date;
            string userId = User.Identity.GetUserId();
            List<Wollo.Entities.Models.User> users = await GetAllUsers();
            ViewBag.Users = users;
            //if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            //{
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilter4?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            //HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);

            }
            else
            {
                // Api failed
            }
            //}
            //else
            //{
            //    //Api Failed
            //}
            ViewBag.UserName = User.Identity.Name;
            Model.CashTransactionModel model = new Model.CashTransactionModel();
            model.CashTransactionHistory = resultObj;
            return View("IssueCash", model);
        }
        //code ends here

        /// <summary>
        /// for request approve
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter5(DateTime start_date, DateTime end_date)
        {
            Approve_Admin_Cash resultObj = new Approve_Admin_Cash();
            //Approve_Admin_Cash master = new Approve_Admin_Cash();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            string userId = User.Identity.GetUserId();
            if (User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/RangeFilter5";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<Approve_Admin_Cash>(responseData);


                }
                else
                {
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.ApproveAdminCashModel model = new Model.ApproveAdminCashModel();
            model.ApproveAdminCash = resultObj;
            return View("CashIssueRequest", model);

        }


        public async Task<ActionResult> CashTrans()
        {
            string userId = User.Identity.GetUserId();
            List<Cash_Transaction_History> lstCashTransactionHistory = new List<Cash_Transaction_History>();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;

            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllCashTransactions";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    lstCashTransactionHistory = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);
                    lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                    {
                        id = x.id,
                        created_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.created_date.Value.ToLocalTime() : x.created_date,
                        updated_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                        created_by = x.created_by,
                        updated_by = x.updated_by,
                        user_id = x.user_id,
                        description = x.description,
                        opening_cash = x.opening_cash,
                        closing_cash = x.closing_cash,
                        transaction_amount = x.transaction_amount,
                        AspNetUsers = x.AspNetUsers
                    }).ToList();
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
                    // Api failed
                }
            }
            else
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllCashTransactionsByUser?userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    lstCashTransactionHistory = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);
                    lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                    {
                        id = x.id,
                        //created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                        //updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                        created_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.created_date.Value.ToLocalTime() : x.created_date,
                        updated_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                        created_by = x.created_by,
                        updated_by = x.updated_by,
                        user_id = x.user_id,
                        description = x.description,
                        opening_cash = x.opening_cash,
                        closing_cash = x.closing_cash,
                        transaction_amount = x.transaction_amount,
                        AspNetUsers = x.AspNetUsers
                    }).ToList();
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
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.CashTransactionModel model = new Model.CashTransactionModel();
            model.CashTransactionHistory = lstCashTransactionHistory;
            return View(model);
        }

        public async Task<ActionResult> CashWithdr()
        {
            List<SelectListItem> lstPaymentMethods = new List<SelectListItem>();
            lstPaymentMethods.Add(new SelectListItem { Text = "Bank Transfer", Value = "Bank Transfer" });
            lstPaymentMethods.Add(new SelectListItem { Text = "Manual", Value = "Manual" });
            lstPaymentMethods.Add(new SelectListItem { Text = "Cheque", Value = "Cheque" });
            ViewBag.PaymentMethodsList = lstPaymentMethods;
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            string userId = User.Identity.GetUserId();
            WithdrawalData resultObj = new WithdrawalData();
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllWithdrawal";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<WithdrawalData>(responseData);
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
                    // Api failed
                }
            }
            else
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllWithdrawalByUser?userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<WithdrawalData>(responseData);
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
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.Withdrawal model = new Model.Withdrawal();
            model.WithdrawalData = resultObj;
            return View(model);
        }

        public async Task<ActionResult> FundTopUp()
        {
            await Task.Yield();
            string userId = User.Identity.GetUserId();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            objuser = await GetUserById(userId);
            List<Wollo.Entities.ViewModels.Payment_Method> lstPaymentMethods = await GetAllPaymentMethods();

            //List<SelectListItem> lstPaymentMethods = new List<SelectListItem>();
            //lstPaymentMethods.Add(new SelectListItem { Text = "Bank Transfer", Value = "Bank Transfer" });
            //lstPaymentMethods.Add(new SelectListItem { Text = "Manual", Value = "Manual" });
            //lstPaymentMethods.Add(new SelectListItem { Text = "Cheque", Value = "Cheque" });
            ViewBag.PaymentMethodsList = lstPaymentMethods;
            TopUpDetails resultObj = new TopUpDetails();
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllTopupDetails";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<TopUpDetails>(responseData);
                    resultObj.AddTopupModel.aes_id = Convert.ToInt32(ConfigurationManager.AppSettings["AESId"]);
                    resultObj.AddTopupModel.paypal_id = Convert.ToInt32(ConfigurationManager.AppSettings["PaypalId"]);
                    resultObj.AddTopupModel.store_id = Convert.ToInt32(ConfigurationManager.AppSettings["StoreId"]);
                    resultObj.AddTopupModel.appid = ConfigurationManager.AppSettings["AppId"];
                    resultObj.AddTopupModel.currency = "USD";
                    resultObj.AddTopupModel.name = objuser.user_name;
                    resultObj.AddTopupModel.user_email = objuser.email_address;
                    resultObj.AddTopupModel.product_name = "Credit";
                    resultObj.AddTopupModel.product_description = "Purchase credits";
                    resultObj.AddTopupModel.quantity = 0;
                    resultObj.AddTopupModel.price = 0;
                    resultObj.AddTopupModel.city = "";
                    resultObj.AddTopupModel.state = "";
                    resultObj.AddTopupModel.country = "";
                    resultObj.AddTopupModel.postcode = "";
                    resultObj.AddTopupModel.address = "";
                    resultObj.AddTopupModel.phone = "";
                    resultObj.AddTopupModel.return_url = ConfigurationManager.AppSettings["ReturnUrl"];
                    resultObj.AddTopupModel.cancel_url = ConfigurationManager.AppSettings["CancelUrl"];
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
                    // Api failed
                }
            }
            else
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllTopupDetailsByUser?userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<TopUpDetails>(responseData);
                    resultObj.AddTopupModel.aes_id = Convert.ToInt32(ConfigurationManager.AppSettings["AESId"]);
                    resultObj.AddTopupModel.paypal_id = Convert.ToInt32(ConfigurationManager.AppSettings["PaypalId"]);
                    resultObj.AddTopupModel.store_id = Convert.ToInt32(ConfigurationManager.AppSettings["StoreId"]);
                    resultObj.AddTopupModel.appid = ConfigurationManager.AppSettings["AppId"];
                    resultObj.AddTopupModel.currency = "USD";
                    resultObj.AddTopupModel.name = objuser.user_name;
                    resultObj.AddTopupModel.user_email = objuser.email_address;
                    resultObj.AddTopupModel.product_name = "Credit";
                    resultObj.AddTopupModel.product_description = "Purchase credits";
                    resultObj.AddTopupModel.quantity = 0;
                    resultObj.AddTopupModel.price = 0;
                    resultObj.AddTopupModel.city = "";
                    resultObj.AddTopupModel.state = "";
                    resultObj.AddTopupModel.country = "";
                    resultObj.AddTopupModel.postcode = "";
                    resultObj.AddTopupModel.address = "";
                    resultObj.AddTopupModel.phone = "";
                    resultObj.AddTopupModel.return_url = ConfigurationManager.AppSettings["ReturnUrl"];
                    resultObj.AddTopupModel.cancel_url = ConfigurationManager.AppSettings["CancelUrl"];
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
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.FundTopup model = new Model.FundTopup();
            model.TopUpDetails = resultObj;
            return View(model);
        }

        [HttpPost]
        public async Task<int> AddTopup(string PaymentMethod, int Amount)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            Topup_History objTopupHistory = new Topup_History();
            objTopupHistory.amount = Amount;
            objTopupHistory.payment_method = PaymentMethod;
            objTopupHistory.user_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/AddTopup";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objTopupHistory);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                objuser = await GetUserById(userId);
                string re = sendMail(objuser, "Topup - Success!", "<div><p>You have made a topup of $" + Amount + ".<br/> Thank you! </p></div>");
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
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

        [HttpGet]
        public async Task<ActionResult> TopupSuccess(Wollo.Web.Models.PaymentSuccess model)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            Topup_History objTopupHistory = new Topup_History();
            decimal number;
            if (Decimal.TryParse(model.Amount, out number))
                objTopupHistory.amount = Convert.ToInt32(number);
            else
                objTopupHistory.amount = 0;
            objTopupHistory.payment_method = model.PaymentType.ToString();
            objTopupHistory.user_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/AddTopup";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objTopupHistory);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                objuser = await GetUserById(userId);
                string re = sendMail(objuser, "Topup - Success!", "<div><p>Topup has been added successfully to your wallet.</p></div>");
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
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            TempData["Success"] = "TopUp added successfully";
            return RedirectToAction("FundTopUp");
        }

        [HttpGet]
        public async Task<ActionResult> TopupCancel(Wollo.Web.Models.PaymentFailure model)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            Topup_History objTopupHistory = new Topup_History();
            //objTopupHistory.amount = model.Amount;
            objTopupHistory.payment_method = model.PaymentType.ToString();
            objTopupHistory.user_id = userId;
            TempData["Success"] = "TopUp Cancelled Successfully";
            return RedirectToAction("FundTopUp");
        }



        [HttpPost]
        public async Task<int> CancelTopup(int id)
        {
            int result = 0;
            ViewBag.UserName = User.Identity.Name;
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                Topup_History objTopup = new Topup_History();
                objTopup.id = id;
                objTopup.user_id = userId;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/CancelTopup";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objTopup);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
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
                    // Api failed
                }
                return result;
            }
            else
            {
                return 3;
            }
        }

        [HttpPost]
        public async Task<int> EditTopup(Topup_History objTopupHistory)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            objTopupHistory.user_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/EditTopup";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objTopupHistory);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
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
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

        [HttpGet]
        public async Task<string> GetWithDrawalFeeByMethod(string WithdrawalMethod)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWithDrawalFeeByMethod?WithdrawalMethod=" + WithdrawalMethod;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            Withdrawl_Fees resultObj = new Withdrawl_Fees();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<Withdrawl_Fees>(responseData);
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
                return responseData;
            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return "";
        }

        [HttpPost]
        public async Task<string> AddWithdrawal(Withdrawel_History_Details objWithdrawelHistoryDetails)
        {
            string[] result = new string[2];
            result[0] = "0";
            string userId = User.Identity.GetUserId();
            objWithdrawelHistoryDetails.withdrawer_user_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/AddWithdrawal";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objWithdrawelHistoryDetails);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<string[]>(responseData);
                if (result[0] == "1")
                {
                    Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                    objuser = await GetUserById(userId);
                    string re = sendMail(objuser, "Withdrawal status!", "<div><p>You have made a withdrawal of $" + objWithdrawelHistoryDetails.amount + ". Your withdrawal will take 4-5 working days for processing. <br/>Thank you! </p></div>");
                }
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
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return string.Join(",", result);
        }

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

        [HttpPost]
        public async Task<string> EditWithdrawal(Withdrawel_History_Details objWithdrawelHistoryDetails)
        {
            string[] result = new string[2];
            result[0] = "0";
            string userId = User.Identity.GetUserId();
            objWithdrawelHistoryDetails.withdrawer_user_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/EditWithdrawal";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objWithdrawelHistoryDetails);
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
            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return string.Join(",", result);
        }

        [HttpPost]
        public async Task<int> CancelWithdrawal(int id)
        {

            int result = 0;
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (userId != null)
            {
                Withdrawel_History_Details objWithdrawelHistoryDetails = new Withdrawel_History_Details();
                objWithdrawelHistoryDetails.id = id;
                objWithdrawelHistoryDetails.withdrawer_user_id = userId;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/CancelWithdrawal";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objWithdrawelHistoryDetails);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
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
                    // Api failed
                }
                return result;
            }
            else
            {
                return 3;
            }
        }

        [HttpPost]
        public async Task<int> ChangeTopupStatus(int id, int status)
        {
            int result = 0;
            ViewBag.UserName = User.Identity.Name;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                string userId = User.Identity.GetUserId();
                Topup_History objTopup = new Topup_History();
                objTopup.id = id;
                objTopup.user_id = userId;
                objTopup.topup_status_id = status;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/ChangeTopupStatus";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objTopup);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
                }
                else
                {
                    // Api failed
                }
                return result;
            }
            else
            {
                return 2;
            }
        }

        [HttpPost]
        public async Task<string> ChangeWithdrawStatus(int id, int status)
        {
            float[] result = new float[2];
            result[0] = 0;
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                Withdrawel_History_Details objWithdrawelHistoryDetails = new Withdrawel_History_Details();
                objWithdrawelHistoryDetails.id = id;
                objWithdrawelHistoryDetails.withdrawer_user_id = userId;
                objWithdrawelHistoryDetails.withdrawel_status_id = status;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/ChangeWithdrawStatus";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objWithdrawelHistoryDetails);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<float[]>(responseData);
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
                    // Api failed
                }
                return string.Join(",", result);
            }
            else
            {
                result[0] = 2;
                return string.Join(",", result);
            }
        }

        //code made by umang 27/06/16 //
        public async Task<ActionResult> CashRequestAdmin()
        {
            string userId = User.Identity.GetUserId();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            List<Issue_Cash_Transfer_Master> resultObj = new List<Issue_Cash_Transfer_Master>();
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllCashDetails?userId=" + userId;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<List<Issue_Cash_Transfer_Master>>(responseData);
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
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = resultObj;
            return View(model);

        }

        public async Task<int> CashAdmin(int Amount)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            Issue_Cash_Transfer_Master objIssueCashTransferMaster = new Issue_Cash_Transfer_Master();
            objIssueCashTransferMaster.cash_issued = Amount;
            objIssueCashTransferMaster.cash_issue_permission_id = 2;
            objIssueCashTransferMaster.receiver_account_id = userId;
            objIssueCashTransferMaster.issuer_account_id = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/CashAdmin";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objIssueCashTransferMaster);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
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
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

        public async Task<ActionResult> IssueCash()
        {
            List<Cash_Transaction_History> resultObj = new List<Cash_Transaction_History>();
            string userId = User.Identity.GetUserId();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
            List<Wollo.Entities.Models.User> users = await GetAllUsers();
            ViewBag.Users = users;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/GetAllIssueCashHistory";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<List<Cash_Transaction_History>>(responseData);
                    if (userId != superAdminId)
                    {
                        resultObj = resultObj.Where(x => x.description.ToLower().Trim() == "cash issued to member by admin" || x.description.ToLower().Trim() == "cash issued to admin by superadmin").ToList();
                    }
                    else
                    {
                        resultObj = resultObj.Where(x => x.description.ToLower() != "cash issued to self by superadmin" && (x.description.ToLower().Trim() == "cash issued to member by admin" || x.description.ToLower().Trim() == "cash issued to admin by superadmin" || x.description.ToLower().Trim() == "cash issued to member by superadmin")).ToList();
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
                    // Api failed
                }
            }
            else
            {
                //Api Failed
            }
            ViewBag.UserName = User.Identity.Name;
            Model.CashTransactionModel model = new Model.CashTransactionModel();
            model.CashTransactionHistory = resultObj;
            return View(model);
        }

        public async Task<List<Wollo.Entities.ViewModels.Payment_Method>> GetAllPaymentMethods()
        {
            List<Wollo.Entities.ViewModels.Payment_Method> lstPaymentMethod = new List<Wollo.Entities.ViewModels.Payment_Method>();
            try
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string GetAllUsersUrl = domain + "api/Master/GetAllPaymentMethods";
                client.BaseAddress = new Uri(GetAllUsersUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.GetAsync(GetAllUsersUrl);
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    lstPaymentMethod = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Payment_Method>>(responseData);

                }
                return lstPaymentMethod;
            }
            catch (Exception e)
            {
                string message = e.InnerException.Message.ToString();
                return lstPaymentMethod;
            }
        }

        public async Task<List<Wollo.Entities.Models.User>> GetAllUsers()
        {
            List<Wollo.Entities.Models.User> lstUser = new List<Entities.Models.User>();
            try
            {
                string userId = User.Identity.GetUserId();
                string adminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string GetAllUsersUrl = domain + "api/Member/GetAllUsers";
                client.BaseAddress = new Uri(GetAllUsersUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.GetAsync(GetAllUsersUrl);
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    lstUser = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
                    lstUser = lstUser.Where(x => x.user_id != userId && x.user_id != adminUserId).OrderBy(y => y.user_name).ToList();
                }
                return lstUser;
            }
            catch (Exception e)
            {
                string message = e.InnerException.Message.ToString();
                return lstUser;
            }

        }

        public async Task<ActionResult> CashIssueRequest()
        {
            //List<Approve_Admin_Cash> resultObj = new List<Approve_Admin_Cash>();
            ViewBag.UserName = User.Identity.Name;
            Approve_Admin_Cash resultObj = new Approve_Admin_Cash();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            string userId = User.Identity.GetUserId();
            if (User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/Approve";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, resultObj);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<Approve_Admin_Cash>(responseData);
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
                    //resultObj = JsonConvert.DeserializeObject<List<Approve_Admin_Cash>>(responseData);

                }
                else
                {
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            Model.ApproveAdminCashModel model = new Model.ApproveAdminCashModel();
            model.ApproveAdminCash = resultObj;
            return View(model);

        }

        [HttpPost]
        public async Task<int> ChangeCashStatus(int id, int status)
        {
            int result = 0;
            ViewBag.UserName = User.Identity.Name;
            string userId = User.Identity.GetUserId();
            if (User.IsInRole("Super Admin 2"))
            {

                Issue_Cash_Transfer_Master objCash = new Issue_Cash_Transfer_Master();
                objCash.id = id;
                objCash.cash_issue_permission_id = status;
                objCash.updated_by = userId;
                objCash.issuer_account_id = userId;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Cash/ChangeCashStatus";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objCash);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
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
                    // Api failed
                }
                return result;
            }
            else
            {
                return 2;
            }
        }

        [HttpPost]
      //  public async Task<int> AddCashMember(Cash_Transaction_History objCashTransactionHistory, string id)
        public async Task<int> AddCashMember( string id , float amount)
        {
            int result = 0;
            float x = amount;
            Cash_Transaction_History objCashTransactionHistory = new Cash_Transaction_History();
            string userId = User.Identity.GetUserId();
            //if (objCashTransactionHistory.AspNetUsers.username != User.Identity.GetUserName())
            if (id != User.Identity.GetUserName())
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                objCashTransactionHistory.updated_by = User.Identity.GetUserId();
                objCashTransactionHistory.transaction_amount = amount;
                string url = domain + "api/Cash/AddCashMember?id=" + id;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objCashTransactionHistory);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
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
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

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
    }
}