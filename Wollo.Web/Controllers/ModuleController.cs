using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wollo.Web;
using Wollo.Web.Helper;
using System.Threading.Tasks;
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ModuleController : Controller
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

        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<module> ModuleList = db.Module.ToList();
                return View(ModuleList);
            }
        }

        public ActionResult Details(int Id)
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ModuleViewModel moduleViewModel = new ModuleViewModel();
                    List<PermissionViewModel> lstPermissionViewModel = new List<PermissionViewModel>();
                    module module = db.Module.Find(Id);
                    moduleViewModel.Id = module.id;
                    moduleViewModel.Name = module.name;
                    List<permission> lstPermission = db.Permission.ToList();
                    foreach (var item in module.ModulePermissionMapping)
                    {
                        PermissionViewModel permissionViewModel = new PermissionViewModel();
                        permissionViewModel.Id = item.permissionid;
                        permissionViewModel.Name = item.Permission.name;
                        lstPermissionViewModel.Add(permissionViewModel);
                    }
                    moduleViewModel.Permissions = lstPermissionViewModel;
                    return View(moduleViewModel);
                }
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Open view containing form to create a new Module
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ModuleViewModel moduleViewModel = new ModuleViewModel();
                    List<PermissionViewModel> lstPermissionViewModel = new List<PermissionViewModel>();
                    List<permission> lstPermission = db.Permission.ToList();
                    foreach (var item in lstPermission)
                    {
                        PermissionViewModel permissionViewModel = new PermissionViewModel();
                        permissionViewModel.Id = item.id;
                        permissionViewModel.Name = item.name;
                        permissionViewModel.IsSelected = false;
                        lstPermissionViewModel.Add(permissionViewModel);
                    }
                    moduleViewModel.Permissions = lstPermissionViewModel;
                    return View(moduleViewModel);
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create an Module and save it in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModuleViewModel moduleViewModel)
        {
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        module Module = new module();
                        Module.name = moduleViewModel.Name;
                        var newModule = db.Module.Add(Module);
                        List<permission> lstPermission = db.Permission.ToList();
                        foreach (var item in moduleViewModel.Permissions)
                        {
                            module_permissions_mapping modulePermissionMapping = new module_permissions_mapping();
                            modulePermissionMapping.moduleid = newModule.id;
                            modulePermissionMapping.Module = newModule;
                            modulePermissionMapping.permissionid = item.Id;
                            modulePermissionMapping.Permission = lstPermission.SingleOrDefault(x => x.id == item.Id);
                            db.ModulePermissionMapping.Add(modulePermissionMapping);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(moduleViewModel);
        }

        /// <summary>
        /// Open view containing form to edit Module
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.UserName = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ModuleViewModel moduleViewModel = new ModuleViewModel();
                List<PermissionViewModel> lstPermissionViewModel = new List<PermissionViewModel>();
                module module = db.Module.Find(Id);
                moduleViewModel.Id = module.id;
                moduleViewModel.Name = module.name;
                List<permission> lstPermission = db.Permission.ToList();
                foreach (var item in lstPermission)
                {
                    PermissionViewModel permissionViewModel = new PermissionViewModel();
                    permissionViewModel.Id = item.id;
                    permissionViewModel.Name = item.name;
                    if (module.ModulePermissionMapping.Any(x => x.permissionid == item.id))
                        permissionViewModel.IsSelected = true;
                    else
                        permissionViewModel.IsSelected = false;
                    lstPermissionViewModel.Add(permissionViewModel);
                }
                moduleViewModel.Permissions = lstPermissionViewModel;
                return View(moduleViewModel);
            }
        }

        /// <summary>
        /// Update Module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ModuleViewModel moduleViewModel)
        {
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        module moduleToUpdate = db.Module.SingleOrDefault(x => x.id == moduleViewModel.Id);
                        moduleToUpdate.name = moduleViewModel.Name;
                        db.Entry(moduleToUpdate).State = EntityState.Modified;
                        //List<Manage_Permission.Models.Action> ActionList = db.Action.ToList();
                        //foreach (var item in model.Actions)
                        //{
                        //    if (item.IsSelected)
                        //    {
                        //        if (!(ModuleToUpdate.ModuleActionMapping.Any(x => x.ActionId == item.Id)))
                        //        {
                        //            ModuleActionMapping ModuleAction = new ModuleActionMapping();
                        //            ModuleAction.ModuleId = ModuleToUpdate.Id;
                        //            ModuleAction.Modules = ModuleToUpdate;
                        //            ModuleAction.ActionId = item.Id;
                        //            ModuleAction.Action = ActionList.SingleOrDefault(x => x.Id == item.Id);
                        //            db.ModuleActionMapping.Add(ModuleAction);
                        //        }                            
                        //    }
                        //    else
                        //    {
                        //        if (ModuleToUpdate.ModuleActionMapping.Any(x => x.ActionId == item.Id))
                        //        {
                        //            db.ModuleActionMapping.Remove(ModuleToUpdate.ModuleActionMapping.SingleOrDefault(x => x.ActionId == item.Id));
                        //        }  
                        //    }
                        //}
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception e)
                {

                }
            }
            return View(moduleViewModel);
        }

        /// <summary>
        /// Delete Module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(int Id)
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    module moduleToDelete = db.Module.Include("ModulePermissionMapping")
                          .SingleOrDefault(x => x.id == Id);
                    List<module_permissions_mapping> lstModulePermissionMapping = moduleToDelete.ModulePermissionMapping.ToList();
                    List<rolemodulepermissionMapping> lstRoleModulePermissionMapping = db.RoleModulePermissionMapping.ToList();
                    if (lstModulePermissionMapping.Count != 0)
                    {
                        foreach (var item in lstModulePermissionMapping)
                        {
                            foreach (var i in lstRoleModulePermissionMapping)
                            {
                                if (i.ModulePermissionMappingId == item.id)
                                {
                                    db.RoleModulePermissionMapping.Remove(i);
                                }
                            }
                            db.ModulePermissionMapping.Remove(item);
                        }
                    }
                    db.Module.Remove(moduleToDelete);
                    db.SaveChanges();
                    return "Success";
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
}