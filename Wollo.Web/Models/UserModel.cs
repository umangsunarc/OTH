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
    public class UserModel : BaseAuditableViewModel
    {
        public List<Wollo.Entities.Models.User> User { get; set; }
        //************************************ Label properties start **********************************//
        [Display(Name = "DashboardManageUsers", ResourceType = typeof(Resource))]
        public string dashboard_manage_users { get; set; }
        [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
        public string manage_users { get; set; }
        [Display(Name = "AddNewUser", ResourceType = typeof(Resource))]
        public string add_new_user { get; set; }
        [Display(Name = "ExporttoCSV", ResourceType = typeof(Resource))]
        public string export_to_csv { get; set; }
        [Display(Name = "Edit", ResourceType = typeof(Resource))]
        public string edit { get; set; }
        [Display(Name = "DeactivateUser", ResourceType = typeof(Resource))]
        public string deactivate_user { get; set; }
        //[Display(Name = "DeactivateUser", ResourceType = typeof(Resource))]
        //public string activate_user { get; set; }
        [Display(Name = "ChangePassword", ResourceType = typeof(Resource))]
        public string change_password { get; set; }
        [Display(Name = "UserRole", ResourceType = typeof(Resource))]
        public string user_role { get; set; }
        [Display(Name = "DashboardEditUser", ResourceType = typeof(Resource))]
        public string dashboard_edit_user { get; set; }
        [Display(Name = "User", ResourceType = typeof(Resource))]
        public string user { get; set; }
        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string Submit { get; set; }
        [Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        public string back_to_users { get; set; }

        //[Display(Name = "ManageRoles", ResourceType = typeof(Resource))]
        //public string manage_roles { get; set; }
        //********************************** End of properties ******************************************//

    }
}