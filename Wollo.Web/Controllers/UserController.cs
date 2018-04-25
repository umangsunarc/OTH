using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wollo.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Wollo.Web.Helper;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using Wollo.Web.Controllers.Helper;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Controllers
{
    //[Authorize]
    public class UserController : BaseController
    {
        public UserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

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

        public async Task<bool> HasSuperAdmin(string roleId)
        {
            bool hasSuperAdmin = false;
            string superAdminRoleId = ConfigurationManager.AppSettings["SuperAdminRoleId"];
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/hasSuperAdmin?roleId=" + superAdminRoleId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                hasSuperAdmin = JsonConvert.DeserializeObject<bool>(responseData);
            }
            return hasSuperAdmin;
        }

        /// <summary>
        /// Soft delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            user.IsDeleted = !user.IsDeleted;
            await UserManager.UpdateAsync(user);
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Member/DeleteUser?userId=" + id;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            int result = 0;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<int>(responseData);
                if (result == 1)
                {
                    if (user.IsDeleted == true)
                    {
                        TempData["Success"] = "User has been deactivated successfully.";
                    }
                    else
                    {
                        TempData["Success"] = "User has been activated successfully.";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Success"] = "User does not exist.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Success"] = "User does not exist.";
                return RedirectToAction("Index");
            }

        }

        //
        // GET: /User/
        [AuthorizeRole(Module = "User", Permission = "View")]
        public async Task<ActionResult> Index()
        {
            UserModel model = new UserModel();
            string userId = User.Identity.GetUserId();
            string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //organizationdetails orgDetails = db.OrganizationDetails.SingleOrDefault(x=>x.IsAdmin==true && x.UserId==userId);
                    //List<ApplicationUser> user = db.Users.Where(x => x.OrganizationDetails.Any(y => y.OrganizationId == orgDetails.OrganizationId && y.IsAdmin==false)).ToList();
                    //List<ApplicationUser> user = db.Users.Where(x => x.UserName.ToLower() != "admin").ToList();
                    HttpClient client = new HttpClient();
                    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                    string GetAllUsersUrl = domain + "api/Member/GetAllUsers";
                    client.BaseAddress = new Uri(GetAllUsersUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Message = await client.GetAsync(GetAllUsersUrl);
                    List<Wollo.Entities.Models.User> lstUser = new List<Entities.Models.User>();
                    if (Message.IsSuccessStatusCode)
                    {
                        var responseData = Message.Content.ReadAsStringAsync().Result;
                        lstUser = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
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
                    if (userId == superAdminId)
                    {
                        lstUser = lstUser.Where(x => x.user_id != "0").ToList();
                    }
                    else
                    {
                        lstUser = lstUser.Where(x => x.user_id != superAdminId).ToList();
                    }
                    model.User = lstUser;
                    return View(model);
                }
            }
            catch (Exception e)
            {

            }
            ViewBag.UserName = User.Identity.Name;
            return View(model);
        }

        public async Task<ActionResult> ExportToCSV()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
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
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    HttpClient client = new HttpClient();
                    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
                    string GetAllUsersUrl = domain + "api/Member/GetAllUsers";
                    client.BaseAddress = new Uri(GetAllUsersUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Message = await client.GetAsync(GetAllUsersUrl);
                    List<Wollo.Entities.Models.User> lstUser = new List<Entities.Models.User>();
                    if (Message.IsSuccessStatusCode)
                    {
                        var responseData = Message.Content.ReadAsStringAsync().Result;
                        lstUser = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
                        DataTable tb1 = new DataTable();
                        //lstUser.FirstOrDefault().
                        tb1.Columns.Add("User Name");
                        tb1.Columns.Add("First Name");
                        tb1.Columns.Add("Last Name");
                        tb1.Columns.Add("Email Address");
                        tb1.Columns.Add("Phone Number");
                        tb1.Columns.Add("Address");
                        tb1.Columns.Add("Alternate Address");
                        tb1.Columns.Add("Country");
                        tb1.Columns.Add("City");
                        tb1.Columns.Add("Zip");
                        foreach (var item in lstUser)
                        {
                            tb1.Rows.Add(item.user_name,
                                item.first_name == null ? "" : item.first_name,
                                item.last_name == null ? "" : item.last_name,
                                item.email_address == null ? "" : item.email_address,
                                item.phone_number == null ? "" : item.phone_number,
                                item.address == null ? "" : item.address,
                                item.alternate_address == null ? "" : item.alternate_address,
                                item.CountryMaster.name == null ? "" : item.CountryMaster.name,
                                item.city == null ? "" : item.city,
                                item.zip == null ? "" : item.zip);
                        }

                        StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = tb1.Columns.Cast<DataColumn>().
                                                          Select(column => column.ColumnName);
                        sb.AppendLine(string.Join(",", columnNames));

                        foreach (DataRow row in tb1.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                            sb.AppendLine(string.Join(",", fields));
                        }

                        var bytes = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(sb.ToString());
                        MemoryStream stream = new MemoryStream(bytes);
                        return File(stream.ToArray(), "text/csv", "Users.csv");

                        //File.WriteAllText("test.csv", sb.ToString());


                        //var userData = new System.Text.StringBuilder();
                        //for (int i = 0; i < tb1.Columns.Count; i++)
                        //{
                        //    userData.Append(tb1.Columns[i].ColumnName);
                        //    userData.Append(i == tb1.Columns.Count - 1 ? "\n" : ",");
                        //}

                        //foreach (DataRow row in tb1.Rows)
                        //{
                        //    for (int i = 0; i < tb1.Columns.Count; i++)
                        //    {
                        //        userData.Append(row[i].ToString());
                        //        userData.Append(i == tb1.Columns.Count - 1 ? "\n" : ",");
                        //    }
                        //}

                        //var bytes = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(result.ToString());
                        //MemoryStream stream = new MemoryStream(bytes);
                        //return File(stream.ToArray(), "text/csv", "Synced.csv");                       
                    }
                }
            }
            catch (Exception e)
            {
                string message = e.InnerException.Message.ToString();
            }
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        //public async Task<ActionResult> ExportToExcel()
        //{
        //    string userId = User.Identity.GetUserId();
        //    ViewBag.UserName = User.Identity.Name;
        //    //*******************************Code to save audit log details start***************************//
        //    Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
        //    logDetails.user_id = userId;
        //    logDetails.created_date = DateTime.UtcNow;
        //    logDetails.updated_date = DateTime.UtcNow;
        //    logDetails.created_by = userId;
        //    logDetails.updated_by = userId;
        //    logDetails.url = HttpContext.Request.Url.AbsoluteUri;
        //    int result = await SaveAuditLogDetails(logDetails);
        //    //*******************************Code to save audit log details start***************************//
        //    try
        //    {
        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {
        //            HttpClient client = new HttpClient();
        //            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
        //            string GetAllUsersUrl = domain + "api/Member/GetAllUsers";
        //            client.BaseAddress = new Uri(GetAllUsersUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            HttpResponseMessage Message = await client.GetAsync(GetAllUsersUrl);
        //            List<Wollo.Entities.Models.User> lstUser = new List<Entities.Models.User>();
        //            if (Message.IsSuccessStatusCode)
        //            {
        //                var responseData = Message.Content.ReadAsStringAsync().Result;
        //                lstUser = JsonConvert.DeserializeObject<List<Wollo.Entities.Models.User>>(responseData);
        //                DataTable tb1 = new DataTable();
        //                //lstUser.FirstOrDefault().
        //                tb1.Columns.Add("User Name");
        //                tb1.Columns.Add("First Name");
        //                tb1.Columns.Add("Last Name");
        //                tb1.Columns.Add("Email Address");
        //                tb1.Columns.Add("Phone Number");
        //                tb1.Columns.Add("Address");
        //                tb1.Columns.Add("Alternate Address");
        //                tb1.Columns.Add("Country");
        //                tb1.Columns.Add("City");
        //                tb1.Columns.Add("Zip");
        //                foreach (var item in lstUser)
        //                {
        //                    tb1.Rows.Add(item.user_name,
        //                        item.first_name == null ? "" : item.first_name,
        //                        item.last_name == null ? "" : item.last_name,
        //                        item.email_address == null ? "" : item.email_address,
        //                        item.phone_number == null ? "" : item.phone_number,
        //                        item.address == null ? "" : item.address,
        //                        item.alternate_address == null ? "" : item.alternate_address,
        //                        item.CountryMaster.name == null ? "" : item.CountryMaster.name,
        //                        item.city == null ? "" : item.city,
        //                        item.zip == null ? "" : item.zip);
        //                }

        //                //Make a new npoi workbook
        //                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        //                //Here I am making sure that I am giving the file name the right
        //                //extension:
        //                string filename = "Users.xls";
        //                //This starts the dialogue box that allows the user to download the file
        //                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
        //                Response.ContentType = "application/vnd.ms-excel";
        //                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        //                Response.Clear();
        //                //make a new sheet – name it any excel-compliant string you want
        //                HSSFSheet sheet1 = hssfworkbook.CreateSheet("Sheet 1");
        //                //make a header row
        //                HSSFRow row1 = sheet1.CreateRow(0);
        //                //Puts in headers (these are table row headers, omit if you
        //                //just need a straight data dump
        //                for (int j = 0; j < tb1.Columns.Count; j++)
        //                {
        //                    HSSFCell cell = row1.CreateCell(j);
        //                    String columnName = tb1.Columns[j].ToString();
        //                    cell.SetCellValue(columnName);
        //                }
        //                //loops through data
        //                for (int i = 0; i < tb1.Rows.Count; i++)
        //                {
        //                    HSSFRow row = sheet1.CreateRow(i + 1);
        //                    for (int j = 0; j < tb1.Columns.Count; j++)
        //                    {
        //                        HSSFCell cell = row.CreateCell(j);
        //                        String columnName = tb1.Columns[j].ToString();
        //                        cell.SetCellValue(tb1.Rows[i][columnName].ToString());
        //                    }
        //                }

        //                //writing the data to binary from memory
        //                Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
        //                Response.End();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string message = e.InnerException.Message.ToString();
        //    }
        //    ViewBag.UserName = User.Identity.Name;
        //    return View();
        //}

        //static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        //{
        //       //Write the stream data of workbook to the root directory
        //       MemoryStream file = new MemoryStream();
        //       hssfworkbook.Write(file);
        //       return file;
        //}

        [AuthorizeRole(Module = "User", Permission = "View")]
        public async Task<ActionResult> Details(string Id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (!String.IsNullOrEmpty(Id))
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        UserViewModel userViewModel = new UserViewModel();
                        ApplicationUser user = UserManager.FindById(Id);
                        userViewModel.Id = user.Id;
                        userViewModel.Name = user.UserName;
                        userViewModel.RoleId = user.Roles.FirstOrDefault().RoleId;
                        userViewModel.Role = user.Roles.FirstOrDefault().Role.Name;
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
                        return View(userViewModel);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException.ToString();
                }
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRole(Module = "User", Permission = "Update")]
        [HttpGet]
        public async Task<ActionResult> Roles(string Id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (!String.IsNullOrEmpty(Id))
            {
                try
                {
                    UserViewModel userViewModel = new UserViewModel();
                    userViewModel.Id = Id;
                    string roleName = UserManager.GetRoles(Id).SingleOrDefault();
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        //List<IdentityRole> lstRoles = db.Roles.Where(x => x.Name != "Developer" && x.Name != "Organization Admin" && x.Name != "Guest").ToList();
                        List<IdentityRole> lstRoles = db.Roles.ToList();
                        string adminRoleId = ConfigurationManager.AppSettings["AdminRoleId"];
                        string superAdminRoleId = ConfigurationManager.AppSettings["SuperAdminRoleId"];
                        string memberRoleId = ConfigurationManager.AppSettings["MemberRoleId"];
                        lstRoles = lstRoles.Where(x => x.Id == adminRoleId || x.Id == superAdminRoleId || x.Id == memberRoleId).ToList();
                        List<SelectListItem> lstSelectRole = new List<SelectListItem>();
                        foreach (IdentityRole role in lstRoles)
                        {
                            if (role.Name == roleName)
                            {
                                lstSelectRole.Add(new SelectListItem { Text = role.Name, Value = role.Name, Selected = true });
                            }
                            else
                            {
                                lstSelectRole.Add(new SelectListItem { Text = role.Name, Value = role.Name });
                            }
                        }
                        ViewBag.Roles = lstSelectRole;
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
                    return View(userViewModel);
                }
                catch (Exception e)
                {

                }
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<JsonResult> IsEmailUnique(string email_address)
        {

            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string Url = domain + "api/Member/FindEmail?email=" + email_address;
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


        [HttpGet]
        public ActionResult CreateUser()
        {

            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Module = "User", Permission = "Update")]
        public async Task<ActionResult> Roles(UserViewModel model)
        {
            var user=await UserManager.FindByIdAsync(model.Id);
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    if (!String.IsNullOrEmpty(model.Role) && !String.IsNullOrEmpty(model.Id))
                    {
                        bool hasSuperAdmin = false;
                        if (model.Role.ToLower().Trim() == "super admin 1")
                        {
                            user.IsAdmin = true;
                            await UserManager.UpdateAsync(user);
                        }
                        else
                        {
                            user.IsAdmin = false;
                            await UserManager.UpdateAsync(user);
                        }
                        //string superAdminRoleId = ConfigurationManager.AppSettings["SuperAdminRoleId"];
                        //string role = UserManager.GetRoles(model.Id).SingleOrDefault().ToLower().Trim();
                        if (model.Role.ToLower().Trim() == "super admin 2")
                        {
                            hasSuperAdmin = await HasSuperAdmin(model.Id);
                        }
                        if (hasSuperAdmin)
                        {
                            TempData["message"] = "Super Admin role is already assigned.";
                        }
                        else
                        {
                            UserManager.RemoveFromRole(model.Id, UserManager.GetRoles(model.Id).SingleOrDefault());
                            UserManager.AddToRole(model.Id, model.Role);
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
                }
                catch (Exception e)
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Module = "User", Permission = "Create")]
        public async Task<ActionResult> CreateUser(CreateUserViewModel model)
        {
            Wollo.Entities.Models.User objUser = new Wollo.Entities.Models.User();
            objUser.first_name = model.first_name;
            objUser.last_name = model.last_name;
            objUser.email_address = model.email_address;
            objUser.password = model.password;
            objUser.user_name = model.user_name;

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
                            int response = await UpdateUser(objUser);
                            if (response == 1)
                            {
                                TempData["Success"] = "A new user has been created.";
                                return RedirectToAction("CreateUser", "User");
                            }
                            else
                            {
                                return View();
                            }
                        }
                        else
                        {
                            return View();
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("user_name", "User Name already exist.");
                }
            }
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    List<organization> lstOrganization = db.Organization.ToList();
            //    List<SelectListItem> selectListOrganization = new List<SelectListItem>();
            //    foreach (organization organization in lstOrganization)
            //    {
            //        selectListOrganization.Add(new SelectListItem { Text = organization.Name, Value = organization.Id.ToString() });
            //    }
            //    ViewBag.lstOrganization = selectListOrganization;
            //}
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetUserByIdUrl = domain + "api/Member/GetUserById?userId=" + Id;
            client.BaseAddress = new Uri(GetUserByIdUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetUserByIdUrl);
            Wollo.Entities.Models.User user = new Entities.Models.User();
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<Wollo.Entities.Models.User>(responseData);
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
            EditUserModel model = new EditUserModel();
            model.User = user;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Wollo.Web.Models.EditUserModel model)
        {
            Wollo.Entities.Models.User user = new Wollo.Entities.Models.User();
            user.user_id = model.User.user_id;
            user.user_name = model.User.user_name;
            user.email_address = model.User.email_address;
            user.first_name = model.User.first_name;
            user.last_name = model.User.last_name;

            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            int result = await UpdateUser(user);
            if (result == 1)
            {
                TempData["Success"] = "Changes have been made successfully.";
                return RedirectToAction("Index");
            }
            else if (result == 2)
            {
                TempData["Success"] = "This email address has been taken by another user.";
                return View(model);
            }
            else
            {
                return View(model);
            }

        }

        [HttpPost]
        public async Task<int> UpdateUser(Wollo.Entities.Models.User model)
        {
            string userId = User.Identity.GetUserId();
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string CreateMemberWalletAndPointsEntry = domain + "api/Member/UpdateUser";
            client.BaseAddress = new Uri(CreateMemberWalletAndPointsEntry);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //RegisterViewModel regModel = new RegisterViewModel();
            //regModel.Email = model.email_address;
            //regModel.Password = model.password;
            //regModel.ConfirmPassword = model.password;
            //regModel.UserId = model.user_id;
            //regModel.UserName = model.user_name;
            HttpResponseMessage Message = await client.PostAsJsonAsync(CreateMemberWalletAndPointsEntry, model);
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

        //    [AuthorizeRole(Module = "User", Permission = "View")]
        //    public async Task<ActionResult> Delete(string Id)
        //    {
        //        string userId = User.Identity.GetUserId();
        //        ViewBag.UserName = User.Identity.Name;
        //        if (!String.IsNullOrEmpty(Id))
        //        {
        //            try
        //            {
        //                using (ApplicationDbContext db = new ApplicationDbContext())
        //                {
        //                    UserViewModel userViewModel = new UserViewModel();
        //                    ApplicationUser user = UserManager.FindById(Id);
        //                    userViewModel.Id = user.Id;
        //                    userViewModel.Name = user.UserName;
        //                    userViewModel.RoleId = user.Roles.FirstOrDefault().RoleId;
        //                    userViewModel.Role = user.Roles.FirstOrDefault().Role.Name;
        //var logins = user.Logins;
        //var rolesForUser = await UserManager.GetRolesAsync(Id);

        //using (var transaction = db.Database.BeginTransaction())
        //{
        //  foreach (var login in logins.ToList())
        //  {
        //    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        //  }

        //  if (rolesForUser.Count() > 0)
        //  {
        //    foreach (var item in rolesForUser.ToList())
        //    {
        //      // item should be the name of the role
        //      var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
        //    }
        //  }

        //  await _userManager.DeleteAsync(user);
        //  transaction.commit();
        //                    //*******************************Code to save audit log details start***************************//
        //                    Wollo.Entities.Models.Audit_Log_Master logDetails = new Entities.Models.Audit_Log_Master();
        //                    logDetails.user_id = userId;
        //                    logDetails.created_date = DateTime.UtcNow;
        //                    logDetails.updated_date = DateTime.UtcNow;
        //                    logDetails.created_by = userId;
        //                    logDetails.updated_by = userId;
        //                    logDetails.url = HttpContext.Request.Url.AbsoluteUri;
        //                    int result = await SaveAuditLogDetails(logDetails);
        //                    //*******************************Code to save audit log details start***************************//
        //                    return View(userViewModel);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                string message = ex.InnerException.ToString();
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
    }
}