using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    //[Authorize]
    public class MemberController : BaseController
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


        //GET: /Member/
        //[AuthorizeRole(Module = "Home", Permission = "View")]
        //[Authorize]
        public async Task<ActionResult> AdminIndex()
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/GetAllAdminDashboardData?userId=" + userId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            DashboardViewModel resultObj = new DashboardViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<DashboardViewModel>(responseData);

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
            HolidayData data = await GetHolidayData();
            resultObj.HolidayData = data;
            ViewBag.UserName = User.Identity.Name;
            Model.Dashboard model = new Model.Dashboard();
            model.DashboardViewModel = resultObj;
            return View(model);
        }

        [AuthorizeRole(Module = "Home", Permission = "View")]
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/GetAllMemberDashboardData?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            DashboardViewModel resultObj = null;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<DashboardViewModel>(responseData);

            }
            HolidayData data = await GetHolidayData();
            resultObj.HolidayData = data;
            ViewBag.UserName = User.Identity.Name;
            Model.Dashboard model = new Model.Dashboard();
            model.DashboardViewModel = resultObj;
            return View(model);
        }

        [AuthorizeRole(Module = "Holiday/Closing Date", Permission = "View")]
        public async Task<HolidayData> GetHolidayData()
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Holiday/GetAllHolidayData";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            HolidayData resultObj = null;
            List<Wollo.Entities.ViewModels.Holiday_Master> holiday = new List<Wollo.Entities.ViewModels.Holiday_Master>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<HolidayData>(responseData);
            }
            DateTime curentDate = DateTime.Now;
            foreach (Wollo.Entities.ViewModels.Holiday_Master data in resultObj.lstHolidayMaster)
            {
                if (data.holiday_statusid == 2)
                {
                    double days = (data.holiday_date - curentDate).TotalDays;
                    if (days <= data.notify_before)
                    {
                        holiday.Add(data);
                    }
                }
            }
            resultObj.lstHolidayMaster = holiday;
            return resultObj;
        }

        /// <summary>
        /// To show cash details
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CashHistoryViewDetail()
        {

            List<Cash_Transaction_Detail> lstCashTransactionHistory = new List<Cash_Transaction_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetCashHistoryViewDetails?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                lstCashTransactionHistory = JsonConvert.DeserializeObject<List<Cash_Transaction_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.CashTransactionDetailViewModel model = new Model.CashTransactionDetailViewModel();
            model.CashTransactionDetails = lstCashTransactionHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        /// <summary>
        /// below code made by umang on 14/09/16 to show reward point details
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RewardPointHistoryViewDetail()
        {
            List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/GetPointHistoryViewDetails?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                lstPointTransactionHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = lstPointTransactionHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        /// <summary>
        /// below code made by umang on 14/09/16 to show test point details
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> TestPointHistoryViewDetail()
        {
            List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/GetTestRewardPointHistoryViewDetails?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                lstPointTransactionHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = lstPointTransactionHistory;
            ViewBag.UserName = User.Identity.Name;
           
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> AccountSettings()
        {

            HttpClient client = new HttpClient();
            ViewBag.UserName = User.Identity.Name;
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetAccountSetting = domain + "api/Member/AccountSettings?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(GetAccountSetting);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetAccountSetting);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                Wollo.Entities.ViewModels.User objUser = new Wollo.Entities.ViewModels.User();
                Wollo.Web.Models.UpdateUserViewModel model = new Model.UpdateUserViewModel();
                objUser = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.User>(responseData);
                model.user_id = objUser.user_id;
                model.user_name = objUser.user_name;
                model.first_name = objUser.first_name;
                model.last_name = objUser.last_name;
                model.email_address = objUser.email_address;
                model.address = objUser.address;
                model.alternate_address = objUser.alternate_address;
                model.city = objUser.city;
                model.country = objUser.country;
                model.country_id = objUser.country_id;
                model.CountryDetails = objUser.CountryDetails;
                model.country_code = objUser.country_code;
                model.zip = objUser.zip;
                Wollo.Web.Models.UpdateUserModel modelupdate = new Model.UpdateUserModel();
                modelupdate.UpdateUserViewModel = model;
                return View(modelupdate);

            }
            else
            {
                //api failed
            }

            return RedirectToAction("AccountSettings");
        }
        [HttpPost]
        public async Task<ActionResult> AccountSettings(Wollo.Web.Models.UpdateUserModel objUser)
        {
            if (ModelState.IsValid)
            {
                Wollo.Entities.ViewModels.User model = new Wollo.Entities.ViewModels.User();
                model.user_id = objUser.UpdateUserViewModel.user_id;
                model.user_name = objUser.UpdateUserViewModel.user_name;
                model.first_name = objUser.UpdateUserViewModel.first_name;
                model.last_name = objUser.UpdateUserViewModel.last_name;
                model.email_address = objUser.UpdateUserViewModel.email_address;
                model.address = objUser.UpdateUserViewModel.address;
                model.alternate_address = objUser.UpdateUserViewModel.alternate_address;
                model.city = objUser.UpdateUserViewModel.city;
                model.country = objUser.UpdateUserViewModel.country;
                model.country_id = objUser.UpdateUserViewModel.country_id;
                model.CountryDetails = objUser.UpdateUserViewModel.CountryDetails;
                model.country_code = objUser.UpdateUserViewModel.country_code;
                model.zip = objUser.UpdateUserViewModel.zip;
                model.created_by = objUser.UpdateUserViewModel.user_id;
                int result = 0;
                string userId = User.Identity.GetUserId();
                HttpClient client = new HttpClient();
                //Wollo.Entities.ViewModels.User UserSetting = new Wollo.Entities.ViewModels.User();//User UserSetting = new User();
                //UserSetting.first_name = user.first_name;
                //UserSetting.last_name = user.last_name;
                //UserSetting.country_code = user.country_code;
                //UserSetting.created_by = User.Identity.GetUserId();
                //UserSetting.phone_number = user.phone_number;
                //UserSetting.address = user.address;
                //UserSetting.alternate_address = user.alternate_address;
                //UserSetting.country_id = user.country_id;
                //UserSetting.city = user.city;
                //UserSetting.zip = user.zip;
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Member/AddUpdateAccountSetting";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, model);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
                    TempData["Success"] = "Personal information has been updated successfully!";
                    return RedirectToAction("AccountSettings");
                }
                else
                {
                    // Api failed
                }
                return RedirectToAction("AccountSettings");
                //return result;
            }
            else
            {
                return View(objUser);
            }

        }

        /// <summary>
        /// Code for topup by member history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TopupByMembersHistory()
        {
            List<Topup_History> topupHistory = new List<Topup_History>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetTopupByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                topupHistory = JsonConvert.DeserializeObject<List<Topup_History>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.TopupHistoryViewModel model = new Model.TopupHistoryViewModel();
            model.TopupHistory = topupHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for witdraw by member history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WithdrawOutByMembersHistory()
        {
            List<Withdrawel_History_Details> withdrawHistory = new List<Withdrawel_History_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWithdrawOutByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                withdrawHistory = JsonConvert.DeserializeObject<List<Withdrawel_History_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.WithdrawelHistoryDetailViewModel model = new Model.WithdrawelHistoryDetailViewModel();
            model.WithdrawelHistoryDetails = withdrawHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for cash issued member history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CashIssuedToMembersHistory()
        {
            List<Issue_Cash_Transfer_Master> issueCashHistory = new List<Issue_Cash_Transfer_Master>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetCashIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                issueCashHistory = JsonConvert.DeserializeObject<List<Issue_Cash_Transfer_Master>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = issueCashHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        ///Code for Wollo Reward Points Transferred In By Members History
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WolloRewardPointsTransferredInByMembersHistory()
        {
            List<Reward_Points_Transfer_Details> rewardPointHistory = new List<Reward_Points_Transfer_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWolloRewardPointsTransferredInByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Reward_Points_Transfer_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.RewardPointsTransferDetailsModel model = new Model.RewardPointsTransferDetailsModel();
            model.RewardPointsTransferDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for Wollo Reward Points Transferred Out By Members History
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WolloRewardPointsTransferredOutByMembersHistory()
        {
            List<Reward_Points_Transfer_Details> rewardPointHistory = new List<Reward_Points_Transfer_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWolloRewardPointsTransferredOutByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Reward_Points_Transfer_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.RewardPointsTransferDetailsModel model = new Model.RewardPointsTransferDetailsModel();
            model.RewardPointsTransferDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for cash issued admin history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CashIssuedIntoSystemHistory()
        {
            List<Issue_Cash_Transfer_Master> issueCashHistory = new List<Issue_Cash_Transfer_Master>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetCashIssuedToAdminHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                issueCashHistory = JsonConvert.DeserializeObject<List<Issue_Cash_Transfer_Master>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = issueCashHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code In Circulation history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CashCirculationHistory()
        {
            List<Wallet_Details> cashHistory = new List<Wallet_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetCashCirculationHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                cashHistory = JsonConvert.DeserializeObject<List<Wallet_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.WalletDetailsViewModel model = new Model.WalletDetailsViewModel();
            model.walletDetails = cashHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for wollo Reward Point In Circulation history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WolloRewardPointsCirculationHistory()
        {
            List<Member_Stock_Details> rewardPointHistory = new List<Member_Stock_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWolloRewardPointCirculationHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Member_Stock_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.MemberStockDetailsViewModel model = new Model.MemberStockDetailsViewModel();
            model.MemberStockDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for wollo Reward Point In Circulation history detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TestRewardPointCirculationHistory()
        {
            List<Member_Stock_Details> rewardPointHistory = new List<Member_Stock_Details>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetTestRewardPointCirculationHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Member_Stock_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.MemberStockDetailsViewModel model = new Model.MemberStockDetailsViewModel();
            model.MemberStockDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for Wollo Reward points issued to members
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WolloRewardPointsIssuedToMembersHistory()
        {
            List<Issue_Point_Transfer_Detail> rewardPointMemberHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWolloRewardPointsIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointMemberHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = rewardPointMemberHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for Wollo Reward points issued into system
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WolloRewardPointsIssuedIntoSystemHistory()
        {
            List<Issue_Point_Transfer_Detail> rewardPointAdminHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetWolloRewardPointsIssuedIntoSystemHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointAdminHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = rewardPointAdminHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for Test Reward points issued to members
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TestRewardPointsIssuedToMembersHistory()
        {
            List<Issue_Point_Transfer_Detail> rewardPointMemberHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetTestRewardPointsIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointMemberHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = rewardPointMemberHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }
        /// <summary>
        /// Code for Test Reward points issued into system
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TestRewardPointsIssuedIntoSystemHistory()
        {
            List<Issue_Point_Transfer_Detail> rewardPointAdminHistory = new List<Issue_Point_Transfer_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetTestRewardPointsIssuedIntoSystemHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointAdminHistory = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = rewardPointAdminHistory;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        //**************************Filtering code by umang 10-11-16 *************************************//
        /// <summary>
        /// Filter For Cash History View detail
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterCashHistoryViewDetail(DateTime start_date, DateTime end_date)
        {

            List<Cash_Transaction_Detail> resultObj = new List<Cash_Transaction_Detail>();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterCashHistoryViewDetail?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Cash_Transaction_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.CashTransactionDetailViewModel model = new Model.CashTransactionDetailViewModel();
            model.CashTransactionDetails = resultObj;
            
            ViewBag.UserName = User.Identity.Name;
            return View("CashHistoryViewDetail", model);
        }


        /// <summary>
        /// Filter For Reward Point History View Detail
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterRewardPointHistoryViewDetail(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/RangeFilterRewardPointHistoryViewDetail?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("RewardPointHistoryViewDetail", model);
        }

        /// <summary>
        /// Filter For Test Point History View detail
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterTestPointHistoryViewDetail(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/RangeFilterTestPointHistoryViewDetail?userId=" + User.Identity.GetUserId();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
          
            return View("TestPointHistoryViewDetail", model);
        }


        /// <summary>
        /// Filter For Cash Issued Into System History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterCashIssuedIntoSystemHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Cash_Transfer_Master> resultObj = new List<Issue_Cash_Transfer_Master>();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterCashIssuedIntoSystemHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("CashIssuedIntoSystemHistory", model);
        }

        /// <summary>
        /// Filter For Wollo Reward Points Issued Into System History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterWolloRewardPointsIssuedIntoSystemHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterWolloRewardPointsIssuedIntoSystemHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("WolloRewardPointsIssuedIntoSystemHistory", model);
        }

        /// <summary>
        /// Filter For Test Reward Points Issued Into System History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterTestRewardPointsIssuedIntoSystemHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterTestRewardPointsIssuedIntoSystemHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("TestRewardPointsIssuedIntoSystemHistory", model);
        }

        /// <summary>
        /// Filter For Cash Issued to Member History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterCashIssuedToMembersHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Cash_Transfer_Master> resultObj = new List<Issue_Cash_Transfer_Master>();
            Issue_Cash_Transfer_Master master = new Issue_Cash_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterCashIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            Model.IssueCashTransferMasterModel model = new Model.IssueCashTransferMasterModel();
            model.IssueCashTransferMaster = resultObj;
            ViewBag.UserName = User.Identity.Name;            
            return View("CashIssuedToMembersHistory", model);
        }

        /// <summary>
        /// Filter For Wollo Reward Points Issued to member History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterWolloRewardPointsIssuedToMembersHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterWolloRewardPointsIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("WolloRewardPointsIssuedToMembersHistory", model);
        }

        /// <summary>
        /// Filter For Test Reward Points Issued to member History
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterTestRewardPointsIssuedToMembersHistory(DateTime start_date, DateTime end_date)
        {

            List<Issue_Point_Transfer_Detail> resultObj = new List<Issue_Point_Transfer_Detail>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterTestRewardPointsIssuedToMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Issue_Point_Transfer_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.IssuePointTransferDetailViewModel model = new Model.IssuePointTransferDetailViewModel();
            model.IssueCashTransferDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("TestRewardPointsIssuedToMembersHistory", model);
        }
        /// <summary>
        /// RangeFilter Wollo Reward Points Transferred In By Members History
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterWolloRewardPointsTransferredInByMembersHistory(DateTime start_date, DateTime end_date)
        {
            List<Reward_Points_Transfer_Details> rewardPointHistory = new List<Reward_Points_Transfer_Details>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterWolloRewardPointsTransferredInByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Reward_Points_Transfer_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.RewardPointsTransferDetailsModel model = new Model.RewardPointsTransferDetailsModel();
            model.RewardPointsTransferDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name;
            return View("WolloRewardPointsTransferredInByMembersHistory", model);
        }
        /// <summary>
        /// Range Filter Wollo Reward Points Transferred Out By Members History
        /// </summary>
        /// <returns></returns> 
        public async Task<ActionResult> RangeFilterWolloRewardPointsTransferredOutByMembersHistory(DateTime start_date, DateTime end_date)
        {
            List<Reward_Points_Transfer_Details> rewardPointHistory = new List<Reward_Points_Transfer_Details>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterWolloRewardPointsTransferredOutByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                rewardPointHistory = JsonConvert.DeserializeObject<List<Reward_Points_Transfer_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.RewardPointsTransferDetailsModel model = new Model.RewardPointsTransferDetailsModel();
            model.RewardPointsTransferDetails = rewardPointHistory;
            ViewBag.UserName = User.Identity.Name; 
            return View("WolloRewardPointsTransferredOutByMembersHistory", model);
        }

        /// <summary>
        /// filter for topup by member history detail
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterTopupByMembersHistory(DateTime start_date, DateTime end_date)
        {
            List<Topup_History> topupHistory = new List<Topup_History>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterTopupByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                topupHistory = JsonConvert.DeserializeObject<List<Topup_History>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.TopupHistoryViewModel model = new Model.TopupHistoryViewModel();
            model.TopupHistory = topupHistory;
            ViewBag.UserName = User.Identity.Name;
            return View("TopupByMembersHistory", model);
        }

        /// <summary>
        /// Filter for witdraw by member history detail
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterWithdrawOutByMembersHistory(DateTime start_date, DateTime end_date)
        {
            List<Withdrawel_History_Details> withdrawHistory = new List<Withdrawel_History_Details>();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterWithdrawOutByMembersHistory";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                withdrawHistory = JsonConvert.DeserializeObject<List<Withdrawel_History_Details>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.WithdrawelHistoryDetailViewModel model = new Model.WithdrawelHistoryDetailViewModel();
            model.WithdrawelHistoryDetails = withdrawHistory;
            ViewBag.UserName = User.Identity.Name;
            return View("WithdrawOutByMembersHistory", model);
        }

        //**************************Filtering code by umang 10-11-16 *************************************//
        /// <summary>
        /// Filter For Cash History View detail
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilterSystemCashDetails(DateTime start_date, DateTime end_date)
        {

            List<Cash_Transaction_Detail> resultObj = new List<Cash_Transaction_Detail>();
            Cash_Transaction_History master = new Cash_Transaction_History();
            master.created_date = start_date;
            master.updated_date = end_date;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/RangeFilterSystemCashDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Cash_Transaction_Detail>>(responseData);
            }
            else
            {
                // Api failed
            }
            Model.CashTransactionDetailViewModel model = new Model.CashTransactionDetailViewModel();
            model.CashTransactionDetails = resultObj;
            ViewBag.UserName = User.Identity.Name;
            return View("GetSystemCashDetails", model);
        }

        /// <summary>
        /// Get all history for Admin Fee, withdrawal fee and Trading Price difference for superadmin
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetSystemCashDetails()
        {
            List<Cash_Transaction_Detail> objTransactionDetails = new List<Cash_Transaction_Detail>();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Cash/GetSystemCashDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                objTransactionDetails = JsonConvert.DeserializeObject<List<Cash_Transaction_Detail>>(responseData);

            }
            else
            {
                // Api failed
            }
            Model.CashTransactionDetailViewModel model = new Model.CashTransactionDetailViewModel();
            model.CashTransactionDetails = objTransactionDetails;
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        //*******************************************end here*******************************************//
    }
}