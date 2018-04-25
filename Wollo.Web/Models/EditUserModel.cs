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
    public class EditUserModel : BaseAuditableViewModel
    {
        public Wollo.Entities.Models.User User { get; set; }
        //*********************************** label properties **************************************************//
        //[Display(Name = "Edit", ResourceType = typeof(Resource))]
        public string edit { get; set; }
        //[Display(Name = "EditUser", ResourceType = typeof(Resource))]
        public string edit_user { get; set; }
        [Display(Name = "DashboardEditUser", ResourceType = typeof(Resource))]
        public string dashboard_edit_user { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }
        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }
        [Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        public string back_to_users { get; set; }
    }
}