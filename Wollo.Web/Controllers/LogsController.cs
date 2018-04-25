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
using System.Collections;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    public class LogsController : BaseController
    {

        public async Task<ActionResult> RangeFilterLogs(DateTime start_date, DateTime end_date)
        {
            
            HttpClient client = new HttpClient();
            Audit_Log_Master master = new Audit_Log_Master();
            List<Wollo.Entities.ViewModels.Audit_Log_Master> resultObj = new List<Wollo.Entities.ViewModels.Audit_Log_Master>();
            master.created_date = start_date;
            master.updated_date = end_date;
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Logs/RangeFilterLogs";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.PostAsJsonAsync(url, master);          
            if (Message.IsSuccessStatusCode)
            {
                TempData["StartDate"] = start_date.Date;
                TempData["EndDate"] = end_date.Date;
                var responseData = Message.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Audit_Log_Master>>(responseData);
            }

            ViewBag.UserName = User.Identity.Name;
            Model.AuditLogMasterModel model = new Model.AuditLogMasterModel();
            model.AuditLogMaster = resultObj;
             return View("Index",model);

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
        // GET: /Logs/
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Logs/GetAllLogDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(url);
            List<Wollo.Entities.ViewModels.Audit_Log_Master> resultObj = new List<Wollo.Entities.ViewModels.Audit_Log_Master>();
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Wollo.Entities.ViewModels.Audit_Log_Master>>(responseData);
            }
            
            ViewBag.user_id = userId;
            ViewBag.UserName = User.Identity.Name;
            //return View(resultObj.Transaction_History);
            Model.AuditLogMasterModel model = new Model.AuditLogMasterModel();
            model.AuditLogMaster = resultObj;
            return View(model);
        }


        /// <summary>
        /// Change Language
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns
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