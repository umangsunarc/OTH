using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wollo.Entities.ViewModels;
using Microsoft.AspNet.Identity;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;
using System.Net.Mail;
using System.Net;
using System.Web;
using Wollo.Web.Common;
using System.Linq;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    //[Authorize(Roles="Admin")]
    public class HolidayController : BaseController
    {
        /// <summary>
        /// Send email from application
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string sendMail(List<Wollo.Entities.Models.User> memberList,Wollo.Entities.Models.User sender, string Subject, string Message)
        {
            try
            {
                Task<string>[] taskArray = { 
                                     Task<string>.Factory.StartNew(() => Mail.sendMailMultiple(memberList,sender, Subject, Message))};
                return "success";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }          
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
        /// for filtering in a date range
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<ActionResult> RangeFilter(DateTime start_date, DateTime end_date)
        {
            Holiday_Master master = new Holiday_Master();
            string userId = User.Identity.GetUserId();
            HolidayData resultObj = new HolidayData();
            HttpClient client = new HttpClient();
            master.created_date = start_date;
            master.updated_date = end_date;
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Holiday/RangeFilter";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage responseMessage = await client.GetAsync(url);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<HolidayData>(responseData);
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
            //ViewBag.UserName = User.Identity.Name;
            Model.HolidayDataModel model = new Model.HolidayDataModel();
            model.HolidayData = resultObj;
            return View("Index", model);
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

        [HttpGet]
        [AuthorizeRole(Module = "Holiday/Closing Date", Permission = "View")]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            TempData["Message"] = null;
            HolidayData resultObj = new HolidayData();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Holiday/GetAllHolidayData";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<HolidayData>(responseData);
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
            ViewBag.UserName = User.Identity.Name;
            Model.HolidayDataModel model = new Model.HolidayDataModel();
            model.HolidayData = resultObj;
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Module = "Holiday/Closing Date", Permission = "Create")]
        public async Task<int> AddHoliday(DateTime holidayDate, string description, int notify)
        {
            string landingPageUrl = ConfigurationManager.AppSettings["LandingPageUrl"];
            string userId = User.Identity.GetUserId();
            int result = 0;
            Holiday_Master objHolidayMaster = new Holiday_Master();
            objHolidayMaster.description = description;
            objHolidayMaster.holiday_date = holidayDate;
            objHolidayMaster.notify_before = notify;
            objHolidayMaster.created_by = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Holiday/AddHoliday";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objHolidayMaster);
            string re = String.Empty;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                List<Wollo.Entities.Models.User> lstUser = new List<Wollo.Entities.Models.User>();
                //objuser = await GetUserById(userId);
                lstUser = await GetAllUsers();
                Wollo.Entities.Models.User sender = lstUser.Where(x => x.user_id == userId).FirstOrDefault();
                lstUser = lstUser.Where(x => x.user_id != userId).ToList();
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
                re = sendMail(lstUser, sender, "New Notification Arrival!", "<div><p>There has been new announcements made. Please navigate to <a href='" + landingPageUrl + "' title='Alternative Asset Excange Platform'>" + landingPageUrl + "</a> to find out the latest news.Thank you!</p></div>");
            }
            else
            {
                // Api failed
            }
            return result;
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
                    //lstUser = lstUser.Where(x => x.user_id != userId && x.user_id != adminUserId).OrderBy(y => y.user_name).ToList();
                }
                return lstUser;
            }
            catch (Exception e)
            {
                string message = e.InnerException.Message.ToString();
                return lstUser;
            }
        }

        [HttpPost]
        [AuthorizeRole(Module = "Holiday/Closing Date", Permission = "Update")]
        public async Task<int> EditHoliday(DateTime holidayDate, string description, int notify, int id)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            Holiday_Master objHolidayMaster = new Holiday_Master();
            objHolidayMaster.id = id;
            objHolidayMaster.description = description;
            objHolidayMaster.holiday_date = holidayDate;
            objHolidayMaster.notify_before = notify;
            objHolidayMaster.updated_by = userId;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Holiday/EditHoliday";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objHolidayMaster);
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

        [HttpPost]
        public async Task<int> ChangeHolidayStatus(int id, int status)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                Holiday_Master objHolidayMaster = new Holiday_Master();
                objHolidayMaster.id = id;
                objHolidayMaster.holiday_statusid = status;
                objHolidayMaster.updated_by = userId;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Holiday/ChangeHolidayStatus";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objHolidayMaster);
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
	}
}