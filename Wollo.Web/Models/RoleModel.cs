using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Base.Entity;
using Wollo.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class RoleModel : BaseAuditableViewModel
    {
        public List<IdentityRole> IdentityRole { get; set; }
        //**************************************** Label properties *****************************************************//
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string roles { get; set; }
        [Display(Name = "DashboardRoles", ResourceType = typeof(Resource))]
        public string dashboard_roles { get; set; }
        [Display(Name = "AddNewRole", ResourceType = typeof(Resource))]
        public string add_new_role { get; set; }
        [Display(Name = "Edit", ResourceType = typeof(Resource))]
        public string edit { get; set; }
        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string name { get; set; }
        [Display(Name = "Options", ResourceType = typeof(Resource))]
        public string options { get; set; }
        [Display(Name = "RolePermissions", ResourceType = typeof(Resource))]
        public string role_permission { get; set; }
        [Display(Name = "DashboardRolePermissions", ResourceType = typeof(Resource))]
        public string dashboard_role_permission { get; set; }
        [Display(Name = "EditPermissions", ResourceType = typeof(Resource))]
        public string edit_permissions { get; set; }
    }
}