using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Wollo.Web;
using Wollo.Web.Models;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Web.Helper;
using Wollo.Base.Utilities;
using Wollo.Base.LocalResource;
using System.Net.Mail;
using System.Net;
using Wollo.Web.Controllers.Helper;
using Wollo.Web.Common;

namespace Wollo.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            //userManager.UserValidator=
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        /// <summary>
        /// To check if email already exist
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<JsonResult> IsEmailUnique(string Email)
        {

            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string Url = domain + "api/Member/FindEmail?email=" + Email;
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(Url);
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                if (responseData == "null")
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);

            }
            return Json(true, JsonRequestBehavior.AllowGet);
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

        public async Task<bool[]> GetUserDetails(LoginViewModel model)
        {
            bool[] isWolloUser = new bool[2];
            Wollo.Entities.Models.User user = new Entities.Models.User();
            user.user_name = model.UserName;
            user.password = model.Password;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/GetUserDetails";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, user);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                isWolloUser = JsonConvert.DeserializeObject<bool[]>(responseData);
            }
            return isWolloUser;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    List<organization> lstOrganization = db.Organization.ToList();
                    List<SelectListItem> selectListOrganization = new List<SelectListItem>();
                    foreach (organization organization in lstOrganization)
                    {
                        selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
                    }
                    ViewBag.lstOrganization = selectListOrganization;
                }
            }
            catch (Exception e)
            {

            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
            //}

        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool[] isWolloUser = await GetUserDetails(model);
                if (isWolloUser[0] == false || (isWolloUser[0]==true && isWolloUser[1]==true))
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {
                        if (!user.IsDeleted)
                        {
                            if (user.OrganizationDetails.Any(x => x.OrganizationId == model.OrganizationId) || user.UserName.ToLower() == "admin")
                            {
                                await SignInAsync(user, model.RememberMe, model.OrganizationId);
                                if (user.UserName.ToLower() == "admin" || user.UserName.ToLower() == "superadmin")
                                {
                                    return RedirectToAction("AdminIndex", "Member");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Member");
                                }
                                //if(returnUrl==null)
                                //{
                                //    return RedirectToLocal(returnUrl);
                                //}
                                //else
                                //{
                                //    return RedirectToAction("Index", "Member");
                                //}                        
                            }
                            else
                            {
                                ModelState.AddModelError("OrganizationId", "This organization does not belong to you.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "This user has been deactivated by admin.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        List<organization> lstOrganization = db.Organization.ToList();
                        List<SelectListItem> selectListOrganization = new List<SelectListItem>();
                        foreach (organization organization in lstOrganization)
                        {
                            selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
                        }
                        ViewBag.lstOrganization = selectListOrganization;
                    }
                }
                else
                {
                    CreateUserViewModel objWolloUser = new CreateUserViewModel();
                    objWolloUser.user_name = model.UserName;
                    objWolloUser.password = model.Password;
                    int result = await CreateUserFromWollo(objWolloUser);
                    if (result == 1)
                    {
                        var user = await UserManager.FindAsync(model.UserName, model.Password);
                        if (user != null)
                        {
                            if (!user.IsDeleted)
                            {
                                    await SignInAsync(user, model.RememberMe, 0);
                                    if (user.UserName.ToLower() == "admin" || user.UserName.ToLower() == "superadmin")
                                    {
                                        return RedirectToAction("AdminIndex", "Member");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Member");
                                    }
                                    //if(returnUrl==null)
                                    //{
                                    //    return RedirectToLocal(returnUrl);
                                    //}
                                    //else
                                    //{
                                    //    return RedirectToAction("Index", "Member");
                                    //}                        
                                
                            }
                            else
                            {
                                ModelState.AddModelError("", "This user has been deactivated by admin.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid username or password.");
                        }
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            List<organization> lstOrganization = db.Organization.ToList();
                            List<SelectListItem> selectListOrganization = new List<SelectListItem>();
                            foreach (organization organization in lstOrganization)
                            {
                                selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
                            }
                            ViewBag.lstOrganization = selectListOrganization;
                        }
                    }
                    //return View(model);
                }               
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Module = "User", Permission = "Create")]
        public async Task<int> CreateUserFromWollo(CreateUserViewModel model)
        {
            int userCreated = 0;
            Wollo.Entities.Models.User objUser = new Wollo.Entities.Models.User();
            objUser.password = model.password;
            objUser.user_name = model.user_name;
            objUser.company_id = 1;

            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.user_name };
                var result = await UserManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    objUser.user_id = user.Id;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        organizationdetails orgDetails = new organizationdetails { OrganizationId = 0, UserId = user.Id, IsAdmin = false };
                        db.OrganizationDetails.Add(orgDetails);
                        db.SaveChanges();
                    }
                    UserManager.AddToRole(user.Id, "Member");
                    HttpClient client = new HttpClient();
                    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                    string CreateMemberWalletAndPointsEntry = domain + "api/Member/CreateMemberWalletAndPointsEntry?userId=" + user.Id;
                    client.BaseAddress = new Uri(CreateMemberWalletAndPointsEntry);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Message = await client.GetAsync(CreateMemberWalletAndPointsEntry);
                    if (Message.IsSuccessStatusCode)
                    {
                        var responseData = Message.Content.ReadAsStringAsync().Result;
                        int data = JsonConvert.DeserializeObject<int>(responseData);
                        if (data == 1)
                        {
                            ////*******************************Code to save audit log details start***************************//
                            //Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
                            //logDetails.user_id = userId;
                            //logDetails.created_date = DateTime.UtcNow;
                            //logDetails.updated_date = DateTime.UtcNow;
                            //logDetails.created_by = userId;
                            //logDetails.updated_by = userId;
                            //logDetails.url = HttpContext.Request.Url.AbsoluteUri;
                            //int r = await SaveAuditLogDetails(logDetails);
                            //*******************************Code to save audit log details start***************************//
                            int response = await UpdateWolloUser(objUser);
                            userCreated = 1;
                        }
                        else
                        {
                            userCreated = 0;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Name already exist.");
                }               
            }
            return userCreated;
        }

        [HttpPost]
        public async Task<int> UpdateWolloUser(Wollo.Entities.Models.User model)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string updateUserUrl = domain + "api/Member/UpdateUser";
            client.BaseAddress = new Uri(updateUserUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //RegisterViewModel regModel = new RegisterViewModel();
            //regModel.Email = model.email_address;
            //regModel.Password = model.password;
            //regModel.ConfirmPassword = model.password;
            //regModel.UserId = model.user_id;
            //regModel.UserName = model.user_name;
            HttpResponseMessage Message = await client.PostAsJsonAsync(updateUserUrl, model);
            int data = 0;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<int>(responseData);
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
            return data;
        }

     
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //List<organization> org = new MasterController().GetAllOrganizations();
                    List<organization> lstOrganization = db.Organization.ToList();
                    List<SelectListItem> selectListOrganization = new List<SelectListItem>();
                    foreach (organization organization in lstOrganization)
                    {
                        selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
                    }
                    ViewBag.lstOrganization = selectListOrganization;
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    model.UserId = user.Id;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        organizationdetails orgDetails = new organizationdetails { OrganizationId = model.OrganizationId, UserId = user.Id, IsAdmin = false };
                        db.OrganizationDetails.Add(orgDetails);
                        db.SaveChanges();
                    }
                    UserManager.AddToRole(user.Id, "Member");
                    await SignInAsync(user, isPersistent: false, organizationId: model.OrganizationId);
                    HttpClient client = new HttpClient();
                    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                    string CreateMemberWalletAndPointsEntry = domain + "api/Member/CreateMemberWalletAndPointsEntry?userId=" + user.Id;
                    client.BaseAddress = new Uri(CreateMemberWalletAndPointsEntry);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Message = await client.GetAsync(CreateMemberWalletAndPointsEntry);
                    if (Message.IsSuccessStatusCode)
                    {
                        var responseData = Message.Content.ReadAsStringAsync().Result;
                        int data = JsonConvert.DeserializeObject<int>(responseData);
                        if (data == 1)
                        {
                            int response = await UpdateUser(model);
                            if (response == 1)
                            {
                                Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
                                objuser.email_address = model.Email;
                                objuser.user_name = model.UserName;
                                string r = sendMail(objuser, "Successful Registration!", "<div><p>Welcome!<br/> Thank you for signing up! You can now start using our trading platform using your login credentials. <br/>Thank you!</p></div>");
                                TempData["Success"] = "You are successfully registered";
                                //return RedirectToAction("Index", "Home");
                                return RedirectToAction("Register", "Account");
                            }
                            else
                            {
                                return RedirectToAction("Register", "Account");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Register", "Account");
                        }
                    }

                }
                else
                {
                    AddErrors(result);
                }
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<organization> lstOrganization = db.Organization.ToList();
                List<SelectListItem> selectListOrganization = new List<SelectListItem>();
                foreach (organization organization in lstOrganization)
                {
                    selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
                }
                ViewBag.lstOrganization = selectListOrganization;
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<int> UpdateUser(RegisterViewModel model)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string UpdateUserUrl = domain + "api/Member/UpdateUser";
            client.BaseAddress = new Uri(UpdateUserUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Wollo.Entities.Models.User user = new Wollo.Entities.Models.User();
            user.email_address = model.Email;
            user.user_name = model.UserName;
            user.user_id = model.UserId;
            HttpResponseMessage Message = await client.PostAsJsonAsync(UpdateUserUrl, user);
            int data = 0;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<int>(responseData);                
            }
            return data;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterOrganization()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterOrganization(RegisterOrganizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        organization org = new organization();
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            org.Name = model.Name;
                            org.Description = model.Description;
                            org = db.Organization.Add(org);
                            organizationdetails orgDetails = new organizationdetails { OrganizationId = org.Id, UserId = user.Id, IsAdmin = true };
                            db.OrganizationDetails.Add(orgDetails);
                            db.SaveChanges();
                        }
                        UserManager.AddToRole(user.Id, "Organization Admin");
                        await SignInAsync(user, isPersistent: false, organizationId: org.Id);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                catch (Exception e)
                {
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            //ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
            message == ManageMessageId.ResetPassword ? "Your password has been changed."
            : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
            : message == ManageMessageId.Error ? "User with this email address does not exist."
            : message == ManageMessageId.ForgotPassword ? "A reset password link has been sent to your email address. Please check your email for further assistance."
            : "";
            //ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Wollo.Entities.Models.User> user = new List<Wollo.Entities.Models.User>();
                //string email = model.Email;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Member/GetAllUsers";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.GetAsync(url);
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
                    if (user.Where(x => x.email_address == model.Email).Any())
                    {
                        Wollo.Entities.Models.User objUser = user.Where(x => x.email_address == model.Email).FirstOrDefault();
                        Cipher objCipher = new Cipher();
                        string token = objCipher.Encrypt(objUser.user_id.ToString());
                        string email = objCipher.Encrypt(model.Email);
                        string resetLink = ConfigurationManager.AppSettings["ResetLink"] + "Account/ResetPassword/?Token=" + token + "&Email=" + email;
                        string result = sendMail(objUser, "Reset Password", "<div><p>Please navigate to below link to reset your password:</p><a href='" + resetLink + "' title='Set Account Passwords'>" + resetLink + "</a><br/><br/>Thank you!</div>");
                        if (result == "success")
                        {
                            return RedirectToAction("ForgotPassword", new { Message = ManageMessageId.ForgotPassword });
                        }
                        else
                        {
                            ViewBag.Error = result;
                            ViewBag.StatusMessage = result;
                            return View(model);
                            //return RedirectToAction("ForgotPassword", new { Message = ManageMessageId.Error, Exception = result });
                        }
                    }
                    else
                    {
                        return RedirectToAction("ForgotPassword", new { Message = ManageMessageId.Error });
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string Token, string Email, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ResetPassword ? "Your password has been reset successfully."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "User with this name does not exist."
                : "";
            //ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
                List<Wollo.Entities.Models.User> userlist = new List<Wollo.Entities.Models.User>();
                //string email = model.Email;
                HttpClient client = new HttpClient();
                string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                string url = domain + "api/Member/GetAllUsers";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Message = await client.GetAsync(url);
                if (Message.IsSuccessStatusCode)
                {
                    var responseData = Message.Content.ReadAsStringAsync().Result;
                    userlist = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
                    if (userlist.Where(x => x.user_id == user.Id).Any())
                    {
                        Wollo.Entities.Models.User objUser = userlist.Where(x => x.user_id == user.Id).FirstOrDefault();
                        if (objUser != null)
                        {

                            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
                            await store.SetPasswordHashAsync(user, hashedNewPassword);
                            await store.UpdateAsync(user);
                            string re = sendMail(objUser, "Reset Password - Success!", "<div><p>Congratulations! Your password has been changed successfully.</p></div>");
                            return RedirectToAction("ResetPassword", new { Message = ManageMessageId.ResetPassword });
                        }
                        else
                        {
                            return RedirectToAction("ResetPassword", new { Message = ManageMessageId.Error });
                        }
                    }
                    else
                    {
                        return RedirectToAction("ForgotPassword", new { Message = ManageMessageId.Error });

                    }
                }
            }
            return View(model);

            //if (ModelState.IsValid)
            //{
            //    //Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            //    //objuser.user_name = model.UserName;
            //    //objuser.email_address = model.Email;
            //    //string re = sendMail(objuser, "Reset Password - Success!", "<div><p>Congratulations! Your password has been changed successfully.</p></div>");
            //    //var user = new ApplicationUser() { UserName = model.UserName};

            //    //var result = await UserManager.CreateAsync( Users.user1=);
            //    //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            //        //List<Wollo.Entities.Models.User> objuser = new List<Wollo.Entities.Models.User>();
            //        ////string email = model.Email;
            //        //HttpClient client = new HttpClient();
            //        //string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            //        //string CreateMemberWalletAndPointsEntry = domain + "api/Member/CreateMemberWalletAndPointsEntry?userId=" + user.Id;
            //        //client.BaseAddress = new Uri(CreateMemberWalletAndPointsEntry);
            //        //client.DefaultRequestHeaders.Accept.Clear();
            //        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //        //HttpResponseMessage Message = await client.GetAsync(CreateMemberWalletAndPointsEntry);

            //        ApplicationDbContext context = new ApplicationDbContext();
            //        UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            //        UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
            //        ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.GetUserId());


            //        if (user != null)
            //        {

            //            //Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            //            //objuser.user_name = model.UserName;
            //            //objuser.user_id = model.UserName;
            //            ////objuser.email_address = model.Email;
            //            //string re = sendMail(objuser, "Reset Password - Success!", "<div><p>Congratulations! Your password has been changed successfully.</p></div>");
            //            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
            //            await store.SetPasswordHashAsync(user, hashedNewPassword);
            //            await store.UpdateAsync(user);
            //            return RedirectToAction("ResetPassword", new { Message = ManageMessageId.ResetPassword });
            //        }
            //        else
            //        {
            //            //Wollo.Entities.Models.User objuser = new Wollo.Entities.Models.User();
            //            //objuser.user_name = model.UserName;
            //            //objuser.email_address = model.Email;
            //            //string re = sendMail(objuser, "Reset Password - Success!", "<div><p>Congratulations! Your password has been changed successfully.</p></div>");
            //            return RedirectToAction("ResetPassword", new { Message = ManageMessageId.Error });
            //        }

            //        //ApplicationDbContext context =new ApplicationDbContext()
            //        //String userId = "<YourLogicAssignsRequestedUserId>";
            //        //String newPassword = "<PasswordAsTypedByUser>";
            //        //ApplicationUser cUser = UserManager.FindById(userId);
            //        //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            //        //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();            
            //        //store.SetPasswordHashAsync(cUser, hashedNewPassword);
            //    }
            //    return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordByAdmin(string id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ResetPassword ? "Password updated successfully."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "User does not exist."
                : "";
            //ViewBag.HasLocalPassword = HasPassword();
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserId = id;
            var user = UserManager.FindById(id);
            ViewBag.UserName = User.Identity.GetUserName();
            ViewBag.MemberName = user.UserName;
            ViewBag.ReturnUrl = Url.Action("ResetPasswordByAdmin");
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPasswordByAdmin(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = await UserManager.FindByIdAsync(model.UserId);
                ViewBag.MemberName = user.UserName;
                ViewBag.UserName = User.Identity.GetUserName();
                if (user != null)
                {
                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
                    await store.SetPasswordHashAsync(user, hashedNewPassword);
                    await store.UpdateAsync(user);
                    return RedirectToAction("ResetPasswordByAdmin", new { Message = ManageMessageId.ResetPassword });
                }
                else
                {
                    return RedirectToAction("ResetPasswordByAdmin", new { Message = ManageMessageId.Error });
                }

                //ApplicationDbContext context =new ApplicationDbContext()
                //String userId = "<YourLogicAssignsRequestedUserId>";
                //String newPassword = "<PasswordAsTypedByUser>";
                //ApplicationUser cUser = UserManager.FindById(userId);
                //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
                //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();            
                //store.SetPasswordHashAsync(cUser, hashedNewPassword);
            }
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                //await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        //await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent, long organizationId)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            //identity.AddClaim(new Claim("OrganizationId", organizationId.ToString()));
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            ResetPassword,
            ForgotPassword,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

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
        /// Send mail to single user
        /// </summary>
        /// <param name="objPortalUser"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
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
            //string htmlMessage;
            //htmlMessage = Resource.EmailTemplate.ToString();
            //htmlMessage = htmlMessage.Replace("&gt;", ">");
            //htmlMessage = htmlMessage.Replace("&lt;", "<");
            //htmlMessage = htmlMessage.Replace("\r", "");
            //htmlMessage = htmlMessage.Replace("\n", "");
            //htmlMessage = htmlMessage.Replace("\t", "");
            //htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
            //using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
            //{
            //    m.To.Add(new System.Net.Mail.MailAddress(user.email_address));
            //    m.Subject = Subject;
            //    m.Body = string.Format(htmlMessage,
            //    user.user_name + " ", Message, DateTime.UtcNow.Year);
            //    m.IsBodyHtml = true;
            //    try
            //    {
            //        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
            //        {
            //            smtp.Send(m);
            //            return "success";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        string message = ex.InnerException.Message.ToString();
            //        return message;
            //    }

            //}
        }
    }
}