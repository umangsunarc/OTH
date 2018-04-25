using Wollo.Entities.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Wollo.Web.Helper;

namespace Wollo.Web.Controllers
{
    public class DummyViewsController : BaseController
    {
        //
        // GET: /DummyViews/
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Test()
        {
            AdminRuleMasterData resultObj = new AdminRuleMasterData();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Master/GetAllRuleMasterData";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<AdminRuleMasterData>(responseData);

                    return Json(resultObj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(responseMessage.Content.ReadAsStreamAsync().Result, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                string stacktrace = ex.InnerException.StackTrace.ToString();
                string message = ex.InnerException.InnerException.Message;
                string source = ex.Source.ToString();
                ViewBag.Exception = message;
                ViewBag.message = message;
                ViewBag.source = source;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Show1()
        {
            return View();
        }

        public ActionResult Show2()
        {
            return View();
        }

        public ActionResult Show3()
        {
            return View();
        }

        public ActionResult Show4()
        {
            return View();
        }

        public ActionResult Show5()
        {
            return View();
        }

        public ActionResult Show6()
        {
            return View();
        }

        public ActionResult Show7()
        {
            return View();
        }
	}
}