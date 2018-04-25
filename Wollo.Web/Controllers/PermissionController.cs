using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wollo.Web;
using Wollo.Web.Controllers.Helper;

namespace Wollo.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        //
        // GET: /Action/
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<permission> lstPermission = db.Permission.ToList();
                return View(lstPermission);
            }
        }

        /// <summary>
        /// Display all the Modules associated with that Permission
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int Id)
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    permission permission = db.Permission.Include("ModulePermissionMapping").SingleOrDefault(x => x.id == Id);
                    PermissionViewModel permissionViewModel = new PermissionViewModel();
                    permissionViewModel.Id = permission.id;
                    permissionViewModel.Name = permission.name;
                    List<ModuleViewModel> lstModuleViewModel = new List<ModuleViewModel>();
                    foreach (var item in permission.ModulePermissionMapping)
                    {
                        ModuleViewModel moduleViewModel = new ModuleViewModel();
                        moduleViewModel.Id = item.moduleid;
                        moduleViewModel.Name = item.Module.name;
                        lstModuleViewModel.Add(moduleViewModel);
                    }
                    permissionViewModel.Modules = lstModuleViewModel;
                    return View(permissionViewModel);
                }
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Open view containing form to create a new Permission
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        /// <summary>
        /// Create an Permission and save it in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(permission permissionModel)
        {
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var newPermission = db.Permission.Add(permissionModel);
                        List<module> lstModule = db.Module.ToList();
                        foreach (var item in lstModule)
                        {
                            module_permissions_mapping modulePermission = new module_permissions_mapping();
                            modulePermission.permissionid = newPermission.id;
                            modulePermission.Permission = newPermission;
                            modulePermission.moduleid = item.id;
                            modulePermission.Module = item;
                            db.ModulePermissionMapping.Add(modulePermission);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(permissionModel);
        }

        /// <summary>
        /// Open view containing form to edit Permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.UserName = User.Identity.Name;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    permission Action = db.Permission.Find(Id);
                    return View(Action);
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(permission permissionModel)
        {
            ViewBag.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        permission ActionToUpdate = db.Permission.Find(permissionModel.id);
                        ActionToUpdate.name = permissionModel.name;
                        db.Entry(ActionToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(permissionModel);
        }

        /// <summary>
        /// Delete Permission
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
                    permission permissionToDelete = db.Permission.Include("ModulePermissionMapping")
                        .SingleOrDefault(x => x.id == Id);
                    List<module_permissions_mapping> modulePermissionMapping = permissionToDelete.ModulePermissionMapping.ToList();
                    List<rolemodulepermissionMapping> roleModulePermissionMapping = db.RoleModulePermissionMapping.ToList();
                    if (modulePermissionMapping.Count != 0)
                    {
                        foreach (var item in modulePermissionMapping)
                        {
                            foreach (var i in roleModulePermissionMapping)
                            {
                                if (i.ModulePermissionMappingId == item.id)
                                {
                                    db.RoleModulePermissionMapping.Remove(i);
                                }
                            }
                            db.ModulePermissionMapping.Remove(item);
                        }
                    }
                    db.Permission.Remove(permissionToDelete);
                    db.SaveChanges();
                    return "Success";
                }
            }
            catch(Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
}