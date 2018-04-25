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
using Wollo.Entities.ViewModels;
using Wollo.Web;
using Wollo.Web.Helper;
using Wollo.Web.Models;
using Wollo.Web.Controllers.Helper;


namespace Wollo.Web.Controllers
{
    public class HomeController : BaseController
    {
        //[AuthorizeRole(Module = "Home", Permission = "View")]
        public ActionResult Index()
        {
            LandingPageViewModel model = new LandingPageViewModel();
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            //HttpClient client = new HttpClient();
            //string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            //string url = domain + "api/Master/GetAllTransactionHistory";
            //client.BaseAddress = new Uri(url);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage responseMessage = await client.GetAsync(url);
            //List<Stock_Code> resultObj = new List<Stock_Code>();
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            //    resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
            //}
            //else
            //{
            //    // Api failed
            //}
            //return View(resultObj);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Facebook()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult UserAgreement()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Twitter()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult ChangeCurrentCulture(int id)
        {
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