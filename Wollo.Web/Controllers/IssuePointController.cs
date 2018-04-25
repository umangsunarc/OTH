using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Entities.ViewModels;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;
using System.Net.Mail;
using System.Net;
using Wollo.Web.Common;
using Model = Wollo.Web.Models;

namespace Wollo.Web.Controllers
{
    //[Authorize]
    public class IssuePointController : BaseController
    {

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

        /// <summary>
        /// Send Email from application to point Isuuer and Receiver
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string sendMailToAdminAndUser(Wollo.Entities.Models.User issuer,Wollo.Entities.Models.User receiver, string Subject, string Message)
        {
            try
            {
                Task<string>[] taskArray = { 
                                     Task<string>.Factory.StartNew(() => Mail.sendMail(issuer, Subject, "Points have been issued successfully.")),
                                     Task<string>.Factory.StartNew(() => Mail.sendMail(receiver, Subject, Message))};
                return "success";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }


        //for filtering in sub-menu self
        public async Task<ActionResult> RangeFilter4(DateTime start_date, DateTime end_date, int stockId)
        {

            IssuePointDetails resultObj = new IssuePointDetails();
            Issue_Points_Master master = new Issue_Points_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            master.stock_code_id = stockId;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/RangeFilter";
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
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);

                }
                else
                {
                    // Api failed
                }
            }
            //resultObj.IssuePointsMaster = resultObj.IssuePointsMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Master
            //{
            //    id = x.id,
            //    user_id = x.user_id,
            //    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
            //    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
            //    created_by = x.created_by,
            //    updated_by = x.updated_by,
            //    issue_points_expiry_date = x.issue_points_expiry_date,
            //    points_issued_date = x.points_issued_date,
            //    StockCode = x.StockCode,
            //    stock_code_id = x.stock_code_id,
            //    transfer_id = x.transfer_id,
            //    status = x.status,
            //    AspnetUsers = x.AspnetUsers,
            //    IssuePointsTransferMaster = x.IssuePointsTransferMaster
            //}).ToList();
            ViewBag.UserName = User.Identity.Name;
            return View("GetIssuePointsHistorySelf", resultObj);
            //for filtering in sub-menu self ;code ends here

        }


        //for admin point issue last submenu of point issue
        public async Task<ActionResult> RangeFilter5(DateTime start_date, DateTime end_date)
        {
            AdminIsuuePointsViewModel resultObj = new AdminIsuuePointsViewModel();
            Issue_Points_Transfer_Master master = new Issue_Points_Transfer_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/RangeFilter1";
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
                    resultObj = JsonConvert.DeserializeObject<AdminIsuuePointsViewModel>(responseData);

                }
                else
                {
                    // Api failed
                }
            }
            resultObj.IssuePointsTransferMaster = resultObj.IssuePointsTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
            {
                id = x.id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issuer_user_id = x.issuer_user_id,
                receiver_user_id = x.receiver_user_id,
                receiver_user_name = x.receiver_user_name,
                AspnetUsers = x.AspnetUsers,
                points_issue_permission_id = x.points_issue_permission_id,
                points_issued = x.points_issued,
                issue_points_on_date = x.issue_points_on_date,
                stockCodeId = x.stockCodeId,
            }).ToList();
            ViewBag.UserName = User.Identity.Name;
            Model.AdminIssuePointModel model = new Model.AdminIssuePointModel();
            model.AdminIsuuePointsViewModel = resultObj;
            return View("AdminPointIssue", model);
        }

    
        public async Task<ActionResult> RangeFilter9(DateTime start_date, DateTime end_date, int stockID)
        {
            IssuePointDetails resultObj = new IssuePointDetails();
            Issue_Points_Master master = new Issue_Points_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            master.stock_code_id = stockID;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/RangeFilter9";
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
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);

                }
                else
                {
                    // Api failed
                }
            }
            resultObj.IssuePointsMaster = resultObj.IssuePointsMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Master
            {
                id = x.id,
                user_id = x.user_id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issue_points_expiry_date = x.issue_points_expiry_date,
                points_issued_date = x.points_issued_date,
                StockCode = x.StockCode,
                stock_code_id = x.stock_code_id,
                transfer_id = x.transfer_id,
                status = x.status,
                AspnetUsers = x.AspnetUsers,
                IssuePointsTransferMaster = x.IssuePointsTransferMaster
            }).ToList();

            ViewBag.UserName = User.Identity.Name;
            //Model.StockCodeModel model = new Model.StockCodeModel();
            //model.StockCode = resultObj;
            return View("GetIssuePointsHistory", resultObj);
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
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Wollo.Entities.Models.User> GetUserByName(string user_name)
        {
            Wollo.Entities.Models.User user = new Wollo.Entities.Models.User();
            //string email = model.Email;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/GetUserByName?user_name=" + user_name;
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
        // GET: /IssuePoint/
        public async Task<ActionResult> Index()
        {
            List<Wollo.Entities.Models.User> users = await GetAllUsers();
            ViewBag.Users = users;
            
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Stock/GetStocks";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            List<Stock_Code> resultObj = new List<Stock_Code>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            else
            {
                // Api failed
            }
            foreach (Stock_Code code in resultObj)
            {
                code.full_name = code.stock_name;
            }
            ViewBag.UserName = User.Identity.Name;
            Model.StockCodeModel model = new Model.StockCodeModel();
            model.StockCode = resultObj;
            model.Users = users;
            return View(model);
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
                    lstUser = lstUser.Where(x => x.user_id != userId && x.user_id != adminUserId).OrderBy(y=>y.user_name).ToList();
                }
                return lstUser;
            }
            catch (Exception e)
            {
                string message = e.InnerException.Message.ToString();
                return lstUser;
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetIssuePointsHistory(int StockCodeId)
        {
            IssuePointDetails resultObj = new IssuePointDetails();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/GetAllIssuePointsHistory?stockCodeId=" + StockCodeId;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);
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
                string url = domain + "api/IssuePoint/GetIssuePointsHistory?stockCodeId=" + StockCodeId + "&userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);
                }
                else
                {
                    // Api failed
                }
            }
            resultObj.IssuePointsMaster = resultObj.IssuePointsMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Master
            {
                id = x.id,
                user_id = x.user_id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issue_points_expiry_date = x.issue_points_expiry_date,
                points_issued_date = x.points_issued_date,
                StockCode = x.StockCode,
                stock_code_id = x.stock_code_id,
                transfer_id = x.transfer_id,
                status = x.status,
                AspnetUsers=x.AspnetUsers,
                IssuePointsTransferMaster = x.IssuePointsTransferMaster
            }).ToList();
            resultObj.IssuePointTransferMaster = resultObj.IssuePointTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
            {
                id = x.id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issuer_user_id = x.issuer_user_id,
                receiver_user_id = x.receiver_user_id,
                receiver_user_name = x.receiver_user_name,
                AspnetUsers = x.AspnetUsers,
                points_issue_permission_id = x.points_issue_permission_id,
                points_issued = x.points_issued,
                issue_points_on_date = x.issue_points_on_date,
                stockCodeId = x.stockCodeId,
            }).ToList();
            ViewBag.UserName = User.Identity.Name;
            return View(resultObj);
        }

        [HttpPost]
        public async Task<int> AddIssuePoint(int stockCodeId, int pointsIssued, string receiverUserName)
        {
            int result = 0;
            Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Issue_Points_Transfer_Master();
            objIssuePointsTransferMaster.stockCodeId = stockCodeId;
            objIssuePointsTransferMaster.receiver_user_name = receiverUserName;
            objIssuePointsTransferMaster.points_issued = pointsIssued;
            if (objIssuePointsTransferMaster.receiver_user_name != User.Identity.GetUserName())
            {
                string userId = User.Identity.GetUserId();
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                objIssuePointsTransferMaster.issuer_user_id = User.Identity.GetUserId();
                string url = domain + "api/IssuePoint/AddIssuePoint";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objIssuePointsTransferMaster);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<int>(responseData);
                    Wollo.Entities.Models.User receiverUser = new Wollo.Entities.Models.User();
                    Wollo.Entities.Models.User issuerUser = await GetUserById(userId);
                    receiverUser = await GetUserByName(objIssuePointsTransferMaster.receiver_user_name);
                    //string re = sendMail(objuser, "Issue Points- Success!", "<div><p>Hi!<br/>A total of " + objIssuePointsTransferMaster.points_issued + " points has been issued to you. <br/>Thank you!</p></div>");
                    if (result == 1)
                    {
                        string re = sendMailToAdminAndUser(issuerUser, receiverUser, "Issue Points- Success!", "<div><p>A total of " + objIssuePointsTransferMaster.points_issued + " points has been issued to you. <br/>Thank you!</p></div>");
                    }                  
                }
                else
                {
                    // Api failed
                }
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

        [HttpPost]
        public async Task<int> EditIssuePoint(int stockCodeId, int pointsIssued, string receiverUserId, int id)
        {
            string userId = User.Identity.GetUserId();
            Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Issue_Points_Transfer_Master();
            objIssuePointsTransferMaster.stockCodeId = stockCodeId;
            objIssuePointsTransferMaster.points_issued = pointsIssued;
            objIssuePointsTransferMaster.receiver_user_id = receiverUserId;
            objIssuePointsTransferMaster.id = id;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            objIssuePointsTransferMaster.updated_by = User.Identity.GetUserId();
            string url = domain + "api/IssuePoint/EditIssuePoint";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objIssuePointsTransferMaster);
            int result = 0;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                objuser = await GetUserById(userId);
                string re = sendMail(objuser, "Issue Points - Modify!", "<div><p>Modifications to issue points field were made successfully.</p></div>");
            }
            else
            {
                // Api failed
            }
            return result;
        }

        [HttpPost]
        public async Task<int> CancelIssuePoint(int id)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Issue_Points_Transfer_Master();
            objIssuePointsTransferMaster.updated_by = User.Identity.GetUserId();
            objIssuePointsTransferMaster.id = id;
            string url = domain + "api/IssuePoint/CancelIssuePoint";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objIssuePointsTransferMaster);
            int result = 0;
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

        //code made by umang 25/07/16

        public async Task<ActionResult> AdminIndex()
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Stock/GetStocks";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            List<Stock_Code> resultObj = new List<Stock_Code>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<List<Stock_Code>>(responseData);
                resultObj = resultObj.Where(x => x.stock_code.ToLower() != "issue points").ToList();
            }
            else
            {
                // Api failed
            }
            foreach (Stock_Code code in resultObj)
            {
                code.full_name =  code.stock_name;
            }
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            Model.StockCodeModel model = new Model.StockCodeModel();
            model.StockCode = resultObj;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetIssuePointsHistorySelf(int StockCodeId)
        {
            IssuePointDetails resultObj = new IssuePointDetails();
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
            {
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/GetAllIssuePointsHistorySelf?stockCodeId=" + StockCodeId;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);
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
                string url = domain + "api/IssuePoint/GetIssuePointsHistory?stockCodeId=" + StockCodeId + "&userId=" + User.Identity.GetUserId();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);
                }
                else
                {
                    // Api failed
                }
            }
            resultObj.IssuePointsMaster = resultObj.IssuePointsMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Master
            {
                id = x.id,
                user_id = x.user_id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issue_points_expiry_date = x.issue_points_expiry_date,
                points_issued_date = x.points_issued_date,
                StockCode = x.StockCode,
                stock_code_id = x.stock_code_id,
                transfer_id = x.transfer_id,
                status = x.status,
                IssuePointsTransferMaster = x.IssuePointsTransferMaster
            }).ToList();
            resultObj.IssuePointTransferMaster = resultObj.IssuePointTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
            {
                id = x.id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issuer_user_id = x.issuer_user_id,
                receiver_user_id = x.receiver_user_id,
                receiver_user_name = x.receiver_user_name,
                AspnetUsers = x.AspnetUsers,
                points_issue_permission_id = x.points_issue_permission_id,
                points_issued = x.points_issued,
                issue_points_on_date = x.issue_points_on_date,
                stockCodeId = x.stockCodeId,
            }).ToList();
            ViewBag.UserName = User.Identity.Name;
            return View(resultObj);
        }

        [HttpPost]
        //public async Task<int> AddIssuePointSelf(Issue_Points_Transfer_Master objIssuePointsTransferMaster)
        public async Task<int> AddIssuePointSelf(int stockCodeId, int pointsIssued)
        {
            Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Issue_Points_Transfer_Master();
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            objIssuePointsTransferMaster.issuer_user_id = ConfigurationManager.AppSettings["SuperAdminUserId"];
            objIssuePointsTransferMaster.receiver_user_id = User.Identity.GetUserId();
            objIssuePointsTransferMaster.receiver_user_name = User.Identity.Name;
            objIssuePointsTransferMaster.stockCodeId = stockCodeId;
            objIssuePointsTransferMaster.points_issued = pointsIssued;
            string url = domain + "api/IssuePoint/AddIssuePointSelf";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objIssuePointsTransferMaster);
            int result = 0;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                objuser = await GetUserById(userId);
                string re = sendMail(objuser, "Issue Points- Success!", "<div><p>Request to issue points has been sent to admin for approval.</p></div>");
            }
            else
            {
                // Api failed
            }
            ViewBag.UserName = User.Identity.Name;
            return result;
        }

        // Code Made By Umang 26/07/16  //

        public async Task<ActionResult> AdminPointIssue()
        {
            TempData["StartDate"] = null;
            TempData["EndDate"] = null;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/GetPointIssueRequest";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            Wollo.Entities.ViewModels.AdminIsuuePointsViewModel lstIssuePointRequest = new Wollo.Entities.ViewModels.AdminIsuuePointsViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                lstIssuePointRequest = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.AdminIsuuePointsViewModel>(responseData);
            }
            lstIssuePointRequest.IssuePointsTransferMaster = lstIssuePointRequest.IssuePointsTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
            {
                id = x.id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issuer_user_id = x.issuer_user_id,
                receiver_user_id = x.receiver_user_id,
                receiver_user_name = x.receiver_user_name,
                AspnetUsers = x.AspnetUsers,
                points_issue_permission_id = x.points_issue_permission_id,
                points_issued = x.points_issued,
                issue_points_on_date = x.issue_points_on_date,
                stockCodeId = x.stockCodeId,
            }).ToList();
            ViewBag.UserName = User.Identity.Name;
            Model.AdminIssuePointModel model = new Model.AdminIssuePointModel();
            model.AdminIsuuePointsViewModel = lstIssuePointRequest;
            return View(model);
        }

        [HttpPost]
        public async Task<int> ChangeSelfIssuePointStatus(int id, int statusId, int stockId)
        {
            Issue_Points_Transfer_Master model = new Issue_Points_Transfer_Master();
            model.id = id;
            model.points_issue_permission_id = statusId;
            model.stockCodeId = stockId;
            model.issuer_user_id = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/IssuePoint/ChangeSelfIssuePointStatus";
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


        public async Task<ActionResult> RangeFilterUser(DateTime start_date, DateTime end_date, int stockID)
        {
            IssuePointDetails resultObj = new IssuePointDetails();
            Issue_Points_Master master = new Issue_Points_Master();
            master.created_date = start_date;
            master.updated_date = end_date;
            master.stock_code_id = stockID;            
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/IssuePoint/RangeFilterUser?userId=" + User.Identity.GetUserId(); 
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, master);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData["StartDate"] = start_date.Date;
                    TempData["EndDate"] = end_date.Date;
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    resultObj = JsonConvert.DeserializeObject<IssuePointDetails>(responseData);

                }      
                else
                {
                    // Api failed
                }
                            
            resultObj.IssuePointsMaster = resultObj.IssuePointsMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Master
            {
                id = x.id,
                user_id = x.user_id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                issue_points_expiry_date = x.issue_points_expiry_date,
                points_issued_date = x.points_issued_date,
                StockCode = x.StockCode,
                stock_code_id = x.stock_code_id,
                transfer_id = x.transfer_id,
                status = x.status,
                AspnetUsers = x.AspnetUsers,
                IssuePointsTransferMaster = x.IssuePointsTransferMaster
            }).ToList();

            ViewBag.UserName = User.Identity.Name;
            return View("GetIssuePointsHistory", resultObj);
        }

    }


}