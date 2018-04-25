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
using Wollo.Web.Controllers.Helper;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    //[Authorize(Roles="Admin")]
    public class RuleController : BaseController
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
            string userId = User.Identity.GetUserId();
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
        // GET: /Rule/
        [AuthorizeRole(Module="Rules",Permission="View")]
        public async Task<ActionResult> Index()
        {
            Model.AdminRuleMasterDataModel model = new Model.AdminRuleMasterDataModel();
            string userId = User.Identity.GetUserId();
            List<SelectListItem> lstPaymentMethods = new List<SelectListItem>();
            lstPaymentMethods.Add(new SelectListItem { Text = "Bank Transfer", Value = "Bank Transfer" });
            lstPaymentMethods.Add(new SelectListItem { Text = "Manual", Value = "Manual" });
            lstPaymentMethods.Add(new SelectListItem { Text = "Cheque", Value = "Cheque" });
            ViewBag.PaymentMethodsList = lstPaymentMethods;

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
                    model.AdminRuleMasterData = resultObj;
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
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    ViewBag.Status = JsonConvert.DeserializeObject<string>(responseData);
                    return View(model);
                }
                ViewBag.UserName = User.Identity.Name;
                return View(model);
            }
            catch (Exception ex)
            {
                string stacktrace = ex.InnerException.StackTrace.ToString();
                string message = ex.InnerException.InnerException.Message;
                string source = ex.Source.ToString();
                ViewBag.Exception = message;
                ViewBag.message = message;
                ViewBag.source = source;
                return View(model);
            }
            
        }

        [HttpPost]
        [AuthorizeRole(Module = "Rules", Permission = "Update")]
        public async Task<int> AddUpdateWithdrawalRule(Withdrawl_Fees objWithdrawlFee)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            objWithdrawlFee.updated_by = User.Identity.GetUserId();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Rule/AddUpdateWithdrawalRule";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objWithdrawlFee);
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
        [AuthorizeRole(Module = "Rules", Permission = "Update")]
        public async Task<int> AddUpdateAdminRule(int AdminStock, int RuleBuyer, int RuleSeller, float FeeBuyer, float FeeSeller)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            List<Administration_Fees> lstAdministrationFees = new List<Administration_Fees>();
            lstAdministrationFees.Add(new Administration_Fees { apply_on = "Buyer", created_by = User.Identity.GetUserId(), fees = FeeBuyer, rule_id = RuleBuyer, stock_code_id = AdminStock });
            lstAdministrationFees.Add(new Administration_Fees { apply_on = "Seller", created_by = User.Identity.GetUserId(), fees = FeeSeller, rule_id = RuleSeller, stock_code_id = AdminStock });
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Rule/AddUpdateAdminRule";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, lstAdministrationFees);
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

        [AuthorizeRole(Module = "Rules", Permission = "Update")]
        public async Task<int> AddUpdateTradingRule(int TradingPointEquivalent, int TradingMinimumLot, int AdminStockTrading, float TradingMinimumRate)
        {
            int result = 0;
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            Units_Master UnitMaster = new Units_Master();
            UnitMaster.points_equivalent = TradingPointEquivalent;
            UnitMaster.minimum_lot = TradingMinimumLot;
            UnitMaster.minimum_rate = TradingMinimumRate;
            UnitMaster.stock_id = AdminStockTrading;
            UnitMaster.created_by = User.Identity.GetUserId();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Rule/AddUpdateTradingRule";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, UnitMaster);
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
        [HttpGet]
        public async Task<ActionResult> GetTradingRule()
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GettradingRule = domain + "api/Rule/GetTradingRule";
            client.BaseAddress = new Uri(GettradingRule);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GettradingRule);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                Wollo.Entities.ViewModels.Units_Master lstTradingRuleHistory = new Wollo.Entities.ViewModels.Units_Master();
                lstTradingRuleHistory = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.Units_Master>(responseData);
                return Json(lstTradingRuleHistory, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //api failed
            }

            return RedirectToAction("Index");
        }
	        
    }
}