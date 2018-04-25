using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    public class RolePermissionController : BaseController
    {
        public RolePermissionController()
            : this(new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext())))
        {
        }

        public RolePermissionController(RoleManager<ApplicationRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public RoleManager<ApplicationRole> RoleManager { get; private set; }

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
        // GET: /UserPermission/
        [AuthorizeRole(Module = "Role Permission", Permission = "View")]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
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

        /// <summary>
        /// Open form to change role permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeRole(Module = "Role Permission", Permission = "Update")]
        public async Task<ActionResult> EditPermission(string Id)
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
                        PermissionViewModel permissionViewModel = new PermissionViewModel();
                        permissionViewModel.Id = i.id;
                        permissionViewModel.Name = i.name;
                        if (lstRoleModulePermissionMapping.Any(x => x.ModulePermissionMapping.permissionid == i.id && x.ModulePermissionMapping.moduleid == item.id))
                            permissionViewModel.IsSelected = true;
                        else
                            permissionViewModel.IsSelected = false;
                        lstPermissionViewModel.Add(permissionViewModel);
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

        [HttpPost]
        [AuthorizeRole(Module = "Role Permission", Permission = "Update")]
        public async Task<ActionResult> EditPermission(RolePermissionViewModel rolePermissionViewModel)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        List<rolemodulepermissionMapping> lstRoleModulePermissionMapping = db.RoleModulePermissionMapping.Include("ModulePermissionMapping").Where(x => x.RoleId == rolePermissionViewModel.RoleId).ToList();
                        ApplicationRole Role = RoleManager.FindById(rolePermissionViewModel.RoleId);
                        List<module_permissions_mapping> lstModulePermissionMapping = db.ModulePermissionMapping.ToList();
                        foreach (var item in rolePermissionViewModel.ModulePermissionMapping)
                        {
                            foreach (var i in item.Permissions)
                            {
                                module_permissions_mapping modulePermissionMapping = lstModulePermissionMapping.SingleOrDefault(x => x.moduleid == item.ModuleId && x.permissionid == i.Id);
                                if (i.IsSelected)
                                {
                                    if (!lstRoleModulePermissionMapping.Any(x => x.ModulePermissionMappingId == modulePermissionMapping.id))
                                    {
                                        rolemodulepermissionMapping roleModulePermissionMapping = new rolemodulepermissionMapping();
                                        roleModulePermissionMapping.RoleId = rolePermissionViewModel.RoleId;
                                        roleModulePermissionMapping.ModulePermissionMappingId = modulePermissionMapping.id;
                                        db.RoleModulePermissionMapping.Add(roleModulePermissionMapping);
                                    }
                                }
                                else
                                {
                                    if (lstRoleModulePermissionMapping.Any(x => x.ModulePermissionMappingId == modulePermissionMapping.id))
                                    {
                                        db.RoleModulePermissionMapping.Remove(lstRoleModulePermissionMapping.SingleOrDefault(x => x.ModulePermissionMappingId == modulePermissionMapping.id));
                                    }
                                }
                            }
                        }
                        db.SaveChanges();
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
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException.ToString();
                }
            }
            return View(rolePermissionViewModel);
        }

        [AuthorizeRole(Module = "Role Permission", Permission = "View")]
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
    }
}