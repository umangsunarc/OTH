using Wollo.Base.Entity;
using Wollo.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels 
{
     [DataContract]
    public class User : BaseAuditableViewModel
    {
         [DataMember]
         public int id { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "user id  is required.")]
         [MaxLength(128, ErrorMessage = "user id cannot be longer than 45 characters.")]
         [MinLength(3, ErrorMessage = "user id cannot be smaller than 3 characters.")]
         public string user_id { get; set; }

         [DataMember]
         [Required(ErrorMessage = "User name  is required.")]
         [MaxLength(45, ErrorMessage = "User name cannot be longer than 45 characters.")]
         [MinLength(3, ErrorMessage = "User name cannot be smaller than 3 characters.")]
         [Display(Name = "UserName", ResourceType = typeof(Resource))]
         public string user_name { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "First name  is required.")]
         //[MaxLength(45, ErrorMessage = "First name cannot be longer than 45 characters.")]
         //[MinLength(3, ErrorMessage = "First name cannot be smaller than 3 characters.")]
         [Display(Name = "FirstName", ResourceType = typeof(Resource))]
         public string first_name { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Last name is required.")]
         //[MaxLength(45, ErrorMessage = "Last name cannot be longer than 45 characters.")]
         //[MinLength(3, ErrorMessage = "Last name cannot be smaller than 3 characters.")]
         [Display(Name = "LastName", ResourceType = typeof(Resource))]
         public string last_name { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Email address  is required.")]
         //[MaxLength(45, ErrorMessage = "Email address cannot be longer than 45 characters.")]
         //[MinLength(3, ErrorMessage = "Email address cannot be smaller than 3 characters.")]
         //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
         [Display(Name = "EmailAddress", ResourceType = typeof(Resource))]
         public string email_address { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "country code  is required.")]
         public string country_code { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Phone number is required.")]
         //[MaxLength(10, ErrorMessage = "Phone number cannot be longer than 10 characters.")]
         //[MinLength(10, ErrorMessage = "Phone number cannot be smaller than 10 characters.")]
         [Display(Name = "PhoneNumber", ResourceType = typeof(Resource))]
         public string phone_number { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Address  is required.")]
         //[MaxLength(150, ErrorMessage = "Address cannot be longer than 150 characters.")]
         //[MinLength(3, ErrorMessage = "Address cannot be smaller than 3 characters.")]
         [Display(Name = "Address", ResourceType = typeof(Resource))]
         public string address { get; set; }

         [DataMember]
         //[MaxLength(150, ErrorMessage = "Alternate address cannot be longer than 150 characters.")]
         //[MinLength(3, ErrorMessage = "Alternate address cannot be smaller than 3 characters.")]
         [Display(Name = "AlternateAddress", ResourceType = typeof(Resource))]
         public string alternate_address { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Country is required.")]
         [ForeignKey("CountryMaster")]
         public int? country_id { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "City  is required.")]
         //[MaxLength(45, ErrorMessage = "City cannot be longer than 45 characters.")]
         //[MinLength(3, ErrorMessage = "City cannot be smaller than 3 characters.")]
         [Display(Name = "City", ResourceType = typeof(Resource))]
         public string city { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Zip  is required.")]
         //[MaxLength(12, ErrorMessage = "Zip cannot be longer than 12 characters.")]
         //[MinLength(3, ErrorMessage = "Zip cannot be smaller than 3 characters.")]
         [Display(Name = "Zip", ResourceType = typeof(Resource))]
         public string zip { get; set; }

         [DataMember]
         //[Required(ErrorMessage = "Password  is required.")]
         //[MaxLength(45, ErrorMessage = "password cannot be longer than 45 characters.")]
         //[MinLength(3, ErrorMessage = "password cannot be smaller than 3 characters.")]
         [Display(Name = "Password", ResourceType = typeof(Resource))]
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
         public virtual Member_Status_Master MemberStatusMaster { get; set; }
         [DataMember]
         public virtual Country_Master CountryMaster { get; set; }
         [DataMember]
         public List<Wollo.Entities.ViewModels.country_master> CountryDetails { get; set; }
         public User()
         {
             CountryDetails = new List<Wollo.Entities.ViewModels.country_master>();
         }


         //****************************** Label Properties ***********************************************//
         [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
         public string manage_users { get; set; }

         [Display(Name = "Users", ResourceType = typeof(Resource))]
         public string users { get; set; }

         [Display(Name = "AddNewUser", ResourceType = typeof(Resource))]
         public string add_new_user { get; set; }

         [Display(Name = "CreateUser", ResourceType = typeof(Resource))]
         public string create_user { get; set; }

         [Display(Name = "User", ResourceType = typeof(Resource))]
         public string user { get; set; }

         [Display(Name = "Create", ResourceType = typeof(Resource))]
         public string create { get; set; }

         [Display(Name = "BackToList", ResourceType = typeof(Resource))]
         public string back_to_list { get; set; }

         [Display(Name = "Status", ResourceType = typeof(Resource))]
         public string status { get; set; }

         [Display(Name = "Options", ResourceType = typeof(Resource))]
         public string options { get; set; }

         [Display(Name = "UserRole", ResourceType = typeof(Resource))]
         public string user_role { get; set; }

         [Display(Name = "ManageRoles", ResourceType = typeof(Resource))]
         public string manage_roles { get; set; }

         [Display(Name = "Country", ResourceType = typeof(Resource))]
         public string country { get; set; }


        //****************************** Label Properties ***********************************************//
    }
}
