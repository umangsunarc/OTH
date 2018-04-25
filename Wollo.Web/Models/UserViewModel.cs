using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.Models;
using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class UserViewModel : BaseAuditableViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        //***************************************** label properties ***********************************************//
        public string assign_roles { get; set; }
        public string manage_role { get; set; }
        public string dashboard_manage_role { get; set; }
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string role_label { get; set; }
        //[Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        //public string select_role { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        [Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        public string back_to_users { get; set; }
        //[Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        public string user_details { get; set; }
        [Display(Name = "DashboardUserDetails", ResourceType = typeof(Resource))]
        public string dashboard_user_details { get; set; }
        [Display(Name = "UserRole",ResourceType=typeof(Resource))]
        public string user_role { get; set; }

    }
}