using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wollo.Web.Models
{
    public class ApplicationRoleModel:BaseAuditableViewModel
    {
        public ApplicationRole ApplicationRole { get; set; }
        //*********************************** Label Properties start ********************************************//
        [Display(Name = "EditRole", ResourceType = typeof(Resource))]
        public string edit_role { get; set; }
        [Display(Name = "DashboardEditRole", ResourceType = typeof(Resource))]
        public string dashboard_edit_role { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        [Display(Name = "BacktoRoles", ResourceType = typeof(Resource))]
        public string back_to_roles { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string name { get; set; }
        //*********************************** Label Properties start ********************************************//
    }
}