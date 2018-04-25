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
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    //[Authorize]
    public class AdminSettingController : BaseController
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
        // GET: /AdminSetting/
        [AuthorizeRole(Module = "Trading Time", Permission = "View")]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            Models.TradingTimeDetailsViewModel model = new Models.TradingTimeDetailsViewModel();
            Wollo.Entities.Models.Trading_Time_Details objTradingTimeDetails = new Entities.Models.Trading_Time_Details();
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
                objTradingTimeDetails = JsonConvert.DeserializeObject<Wollo.Entities.Models.Trading_Time_Details>(responseData);
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
            List<WeakDays> weakDays= await GetWeakDays();
            List<int> selectedDays=new List<int>();
            //for(int i=0;i<weakDays.Count;i++){
            //    if(weakDays[i].IsSelected==true)
            //    {
            //        selectedDays.Add(weakDays[i].Id);
            //    }
            //}
            //result.Days = weakDays.ToArray();
            model.Days = weakDays;
            model.start_time = objTradingTimeDetails.start_time;
            model.end_time = objTradingTimeDetails.end_time;
            //result.selected_days = selectedDays.ToArray();
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        [HttpGet]
        public async Task<List<WeakDays>> GetWeakDays()
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/AdminSetting/GetWeakDays";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            List<Wollo.Entities.Models.trading_days> result = new List<Wollo.Entities.Models.trading_days>();
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.trading_days>>(responseData);
            }
            List<WeakDays> days = new List<WeakDays>();
            foreach (Wollo.Entities.Models.trading_days data in result)
            {
                WeakDays day = new WeakDays();
                day.Id = data.id;
                day.Name = data.name;
                day.IsSelected = data.is_selected;
                days.Add(day);
            }
            return days;
        }

        [HttpPost]
        [AuthorizeRole(Module = "Trading Time", Permission = "Update")]
        public async Task<ActionResult> Index(Trading_Time_Details model)
        {
            model.user_id = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/AdminSetting/UpdateTradingTime";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.PostAsJsonAsync(url, model);
            int result = 0;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
            }
            ViewBag.UserName = User.Identity.Name;
            return RedirectToAction("Index","AdminSetting");
        }

        [HttpGet]
        public async Task<ActionResult> SetMarketRate()
        {
            string userId = User.Identity.GetUserId();
            Wollo.Entities.Models.Market_Rate_Details result = new Wollo.Entities.Models.Market_Rate_Details();
            Market_Rate_Details details = new Market_Rate_Details();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/AdminSetting/GetCurrentMarketRateDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<Wollo.Entities.Models.Market_Rate_Details>(responseData);
                details.rate = result.rate;
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
            ViewBag.UserName = User.Identity.Name;
            Model.MarketRateModel model = new Model.MarketRateModel();
            model.MarketRateDetails = result;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SetMarketRate(Wollo.Web.Models.MarketRateModel model)
        {
            if (ModelState.IsValid)
            {
                Wollo.Entities.Models.Market_Rate_Details objDetails = new Entities.Models.Market_Rate_Details();
                objDetails.rate = model.MarketRateDetails.rate;
                objDetails.created_by = User.Identity.GetUserId();
                //model.created_by = User.Identity.GetUserId();
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/AdminSetting/UpdateMarketRate";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.PostAsJsonAsync(url, objDetails);
                int result = 0;
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
                }
                ViewBag.UserName = User.Identity.Name;
                return RedirectToAction("SetMarketRate", "AdminSetting");
            }
            else
            {
                ViewBag.Message = "Please insert a valid Market Rate";
                return View(model);
            }
        }
    }
}