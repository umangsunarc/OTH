using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class User : AuditableEntity
    {
        [DataMember]
        //[Required(ErrorMessage = "user id  is required.")]
        [MaxLength(45, ErrorMessage = "user id cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "user id cannot be smaller than 3 characters.")]
        public string user_id { get; set; }

        [DataMember]
        [Display(Name = "Uname", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "User name  is required.")]
        [MaxLength(45, ErrorMessage = "User name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "User name cannot be smaller than 3 characters.")]
        public string user_name { get; set; }

        [DataMember]
        [Display(Name = "FirstName",ResourceType=typeof(Resource))]
        //[Required(ErrorMessage = "First name  is required.")]
        //[MaxLength(45, ErrorMessage = "First name cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "First name cannot be smaller than 3 characters.")]
        public string first_name { get; set; }

        [DataMember]
        [Display(Name = "LastName",ResourceType=typeof(Resource))]
        //[Required(ErrorMessage = "Last name is required.")]
        //[MaxLength(45, ErrorMessage = "Last name cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "Last name cannot be smaller than 3 characters.")]
        public string last_name { get; set; }

        [DataMember]
        [Display(Name = "EmailAddress",ResourceType=typeof(Resource))]
        //[Required(ErrorMessage = "Email address  is required.")]
        //[MaxLength(45, ErrorMessage = "Email address cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "Email address cannot be smaller than 3 characters.")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string email_address { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "country code  is required.")]
         public string country_code { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "phone_number is required.")]
        //[MaxLength(10, ErrorMessage = "phone number cannot be longer than 10 characters.")]
        //[MinLength(10, ErrorMessage = "phone number cannot be smaller than 10 characters.")]       
        public string phone_number { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "address  is required.")]
        //[MaxLength(150, ErrorMessage = "address cannot be longer than 150 characters.")]
        //[MinLength(3, ErrorMessage = "address cannot be smaller than 3 characters.")]
        public string address { get; set; }

        [DataMember]
        //[MaxLength(150, ErrorMessage = "alternate address cannot be longer than 150 characters.")]
        //[MinLength(3, ErrorMessage = "alternate address cannot be smaller than 3 characters.")]
        public string alternate_address { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "country id  is required.")]
        [ForeignKey("CountryMaster")]
        public int? country_id { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "city  is required.")]
        //[MaxLength(45, ErrorMessage = "city cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "city cannot be smaller than 3 characters.")]
        public string city { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "zip  is required.")]
        //[MaxLength(12, ErrorMessage = "zip cannot be longer than 12 characters.")]
        //[MinLength(3, ErrorMessage = "zip cannot be smaller than 3 characters.")]
        public string zip { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "Password  is required.")]
        //[MaxLength(45, ErrorMessage = "password cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "password cannot be smaller than 3 characters.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "user statusid  is required.")]
        [ForeignKey("MemberStatusMaster")]
        public int user_statusid { get; set; }

        [DataMember]
        //[Required(ErrorMessage = "company id  is required.")]       
        public int company_id { get; set; }

        [DataMember]
        public bool is_deleted { get; set; }

        [DataMember]
        public virtual Member_Status_Master MemberStatusMaster {get;set;}
        [DataMember]
        public virtual Country_Master CountryMaster { get; set; }

        //*********************************** label properties start **************************************************//
        //[Display(Name = "ManageUsers",ResourceType=typeof(Resource))]
        //public string manage_users { get; set; }

        //[Display(Name="DashboardManageUsers",ResourceType=typeof(Resource))]
        //public string dashboard_manage_users { get; set; }

        //[Display(Name = "AddNewUser", ResourceType = typeof(Resource))]
        //public string add_new_user { get; set; }

        //[Display(Name = "ExportToCSV", ResourceType = typeof(Resource))]
        //public string export_to_csv { get; set; }

        //[Display(Name = "Edit", ResourceType = typeof(Resource))]
        //public string edit { get; set; }

        //[Display(Name = "DeactivateUser", ResourceType = typeof(Resource))]
        //public string deactivate_user { get; set; }

        //[Display(Name = "ChangePassword", ResourceType = typeof(Resource))]
        //public string change_password { get; set; }

        //[Display(Name = "UserRole", ResourceType = typeof(Resource))]
        //public string user_role { get; set; }

        //[Display(Name = "ManageRoles", ResourceType = typeof(Resource))]
        //public string manage_roles { get; set; }

    }
}
