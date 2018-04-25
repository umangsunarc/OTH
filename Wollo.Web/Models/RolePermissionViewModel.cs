using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class RolePermissionViewModel : BaseAuditableViewModel
    {
        public string RoleId { get; set; }
        public string Role { get; set; }
        public List<ModulePermissionMappingViewModel> ModulePermissionMapping { get; set; }
        //*************************************** label properties ***********************************************//
        [Display(Name = "RoleDetails", ResourceType = typeof(Resource))]
        public string role_details { get; set; }
        [Display(Name = "DashboardRoleDetails", ResourceType = typeof(Resource))]
        public string dashboard_role_details { get; set; }
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string role_label { get; set; }
        [Display(Name = "Module", ResourceType = typeof(Resource))]
        public string module { get; set; }
        [Display(Name = "BacktoRoles", ResourceType = typeof(Resource))]
        public string back_to_role { get; set; }
        [Display(Name = "Permissions", ResourceType = typeof(Resource))]
        public string permissions { get; set; }
        [Display(Name = "UpdateRolePermission", ResourceType = typeof(Resource))]
        public string update_role_permission { get; set; }
        [Display(Name = "DashboardUpdateRolePermission", ResourceType = typeof(Resource))]
        public string dashboard_update_role_permission { get; set; }
        [Display(Name = "DashboardRolePermissionDetails", ResourceType = typeof(Resource))]
        public string dashboard_role_permission_details { get; set; }
        [Display(Name = "RolePermissionDetails", ResourceType = typeof(Resource))]
        public string role_permission_details { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        [Display(Name = "BacktoRolePermissions", ResourceType = typeof(Resource))]
        public string back_to_role_permission { get; set; }

    }
}