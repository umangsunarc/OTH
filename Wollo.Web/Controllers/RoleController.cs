using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wollo.Web;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : BaseController
    {

        public RoleController()
            : this(new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext())))
        {
        }

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public RoleManager<ApplicationRole> RoleManager { get; private set; }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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
        // GET: /Role/
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    List<IdentityRole> lstRoles = db.Roles.ToList();
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
                    RoleModel model = new RoleModel();
                    model.IdentityRole = lstRoles;
                    return View(model);
                }
            }
            catch (Exception e)
            {

            }
            return View();
        }

        public async Task<ActionResult> Details(string Id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                RolePermissionViewModel rolePermissionViewModel = new RolePermissionViewModel();
                List<rolemodulepermissionMapping> lstRoleModulePermissionMapping = db.RoleModulePermissionMapping.Include("ModulePermissionMapping").Where(x => x.RoleId == Id).ToList();
                rolePermissionViewModel.RoleId = Id;
                rolePermissionViewModel.Role = db.Roles.SingleOrDefault(x => x.Id == Id).Name;
                List<module> lstModule = db.Module.ToList();
                List<permission> lstPermission = db.Permission.ToList();
                List<ModulePermissionMappingViewModel> lstModulePermissionMappingViewModel = new List<ModulePermissionMappingViewModel>();
                foreach (var item in lstModule)
                {
                    ModulePermissionMappingViewModel modulePermissionMappingViewModel = new ModulePermissionMappingViewModel();
                    modulePermissionMappingViewModel.Id = item.id;
                    modulePermissionMappingViewModel.ModuleId = item.id;
                    modulePermissionMappingViewModel.Name = item.name;
                    module module = new module();
                    List<PermissionViewModel> lstPermissionViewModel = new List<PermissionViewModel>();
                    foreach (var i in lstPermission)
                    {
            
            PermissionViewModel permission = new PermissionViewModel();
                        permission.Id = i.id;
                        permission.Name = i.name;
                        if (lstRoleModulePermissionMapping.Any(x => x.ModulePermissionMapping.permissionid == i.id && x.ModulePermissionMapping.moduleid == item.id))
                            permission.IsSelected = true;
                        else
                            permission.IsSelected = false;
                        lstPermissionViewModel.Add(permission);
                    }
                    modulePermissionMappingViewModel.Permissions = lstPermissionViewModel;
                    modulePermissionMappingViewModel.Module = module;
                    lstModulePermissionMappingViewModel.Add(modulePermissionMappingViewModel);
                }
                rolePermissionViewModel.ModulePermissionMapping = lstModulePermissionMappingViewModel;
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
                return View(rolePermissionViewModel);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleModel)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationRole role = new ApplicationRole { Name = roleModel.Name };
                    var roleresult = await RoleManager.CreateAsync(role);
                    if (roleresult.Succeeded)
                    {
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrors(roleresult);
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(roleModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                if (!String.IsNullOrEmpty(Id))
                {
                    ApplicationRole role = await RoleManager.FindByIdAsync(Id);
                    ApplicationRoleModel model = new ApplicationRoleModel();
                    model.ApplicationRole = role;
                    return View(model);
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationRoleModel roleModel)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    var role = RoleManager.FindById(roleModel.ApplicationRole.Id);
                    role.Name = roleModel.ApplicationRole.Name;
                    var roleresult = await RoleManager.UpdateAsync(role);
                    if (roleresult.Succeeded)
                    {
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrors(roleresult);
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(roleModel);
        }

        [HttpPost]
        public async Task<string> Delete(string Id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            try
            {
                if (!String.IsNullOrEmpty(Id))
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        IdentityRole role = db.Roles.Find(Id);
                        db.Roles.Remove(role);
                        await db.SaveChangesAsync();
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
                        return "Success";
                    }
                }
                return "Id cannot be null";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
}