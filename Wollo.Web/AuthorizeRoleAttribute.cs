using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Wollo.Web
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        // Custom property
        public string Permission { get; set; }
        public string Module { get; set; }
        public string RedirectUrl = "~/UnAuthorized/Index";
        private const string IS_AUTHORIZED = "isAuthorized";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string returnurl = httpContext.Request.Url.AbsolutePath;
                using (UserManager<ApplicationUser> userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    var userRole = userManager.GetRoles(userManager.FindByName(httpContext.User.Identity.Name).Id).SingleOrDefault();
                    List<rolemodulepermissionMapping> lstRoleModulePermissionMapping = db.RoleModulePermissionMapping.Where(x => x.Role.Name == userRole).ToList();
                    foreach (rolemodulepermissionMapping item in lstRoleModulePermissionMapping)
                    {
                        if (item.ModulePermissionMapping.Permission.name == this.Permission && item.ModulePermissionMapping.Module.name == this.Module)
                        {
                            httpContext.Items.Add(IS_AUTHORIZED, true);
                            return isAuthorized;
                            //return true;
                        }
                    }
                    //return false;
                    httpContext.Items.Add(IS_AUTHORIZED, false);
                    return isAuthorized;
                    //return httpContext.Response.Redirect(RedirectUrl);
                }
            }
        }




        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var isAuthorized = filterContext.HttpContext.Items[IS_AUTHORIZED] != null
                ? Convert.ToBoolean(filterContext.HttpContext.Items[IS_AUTHORIZED])
                : false;

            if (!isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUrl);
            }
        }
    }
}