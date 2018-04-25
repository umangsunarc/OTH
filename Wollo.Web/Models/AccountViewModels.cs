using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName",ResourceType=typeof(Resource))]
        public string UserName { get; set; }

        //[Required]
        [Display(Name = "EmailAddress",ResourceType=typeof(Resource))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password",ResourceType=typeof(Resource))]
        public string Password { get; set; }

        [Display(Name = "Rememberme", ResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }

        //[Required]
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }

        //************************************Label Properties *******************************************//
        [Display(Name = "Home", ResourceType = typeof(Resource))]
        public string home { get; set; }

        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string about_us { get; set; }

        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string contact_us { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }
        [Display(Name = "ForgotPassword", ResourceType = typeof(Resource))]
        public string forgot_password { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string Reset { get; set; }

        //*********************************** Label Properties******************************************//
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserName",ResourceType=typeof(Resource))]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(45, ErrorMessage = "Email address cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Email address cannot be smaller than 3 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [System.Web.Mvc.Remote("IsEmailUnique", "Account", ErrorMessage = "A user with this email already exist.")]
        [Display(Name = "EmailAddress",ResourceType=typeof(Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password",ResourceType=typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword",ResourceType=typeof(Resource))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        //************************************Label Properties *******************************************//
        [Display(Name = "Home", ResourceType = typeof(Resource))]
        public string home { get; set; }

        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string about_us { get; set; }

        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string contact_us { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }
        [Display(Name = "Signup", ResourceType = typeof(Resource))]
        public string signup { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        //[Display(Name = "Register", ResourceType = typeof(Resource))]
        public string user_agreement { get; set; }
        //*********************************** Label Properties******************************************//
    }

    public class RegisterOrganizationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Organization")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Description { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "EmailAddress",ResourceType=typeof(Resource))]
        public string Email { get; set; }


        //************************************Label Properties *******************************************//
        [Display(Name = "Home", ResourceType = typeof(Resource))]
        public string home { get; set; }

        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string about_us { get; set; }

        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string contact_us { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }
        [Display(Name = "RESETPASSWORD", ResourceType = typeof(Resource))]
        public string reset_password { get; set; }
        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        //*********************************** Label Properties******************************************//
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        //************************************Label Properties *******************************************//
        [Display(Name = "Home", ResourceType = typeof(Resource))]
        public string home { get; set; }

         [Display(Name = "RESETPASSWORD", ResourceType = typeof(Resource))]
        public string resetpassword { get; set; }

        

        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string about_us { get; set; }

        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string contact_us { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }

        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        //*********************************** Label Properties******************************************//
    }

    public class ChangePasswordViewModel : BaseAuditableViewModel
    {
        [Required]
        [Display(Name = "User name")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserId { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        //************************************Label Properties *******************************************//
        [Display(Name = "Home", ResourceType = typeof(Resource))]
        public string home { get; set; }

        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string about_us { get; set; }

        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string contact_us { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }
        [Display(Name = "ChangePassword", ResourceType = typeof(Resource))]
        public string change_password { get; set; }
        [Display(Name = "DashboardChangePassword", ResourceType = typeof(Resource))]
        public string dashboard_change_password { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string password_label { get; set; }
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string confirm_password { get; set; }
        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }

        //*********************************** Label Properties******************************************//
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "User name  is required.")]    
        [StringLength(45, MinimumLength = 3, ErrorMessage = "User name cannot be smaller than 3 and longer than 45 characters.")]     
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        [Required(ErrorMessage = "First name  is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "First name cannot be smaller than 3 and longer than 45 characters.")]      
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Last name cannot be smaller than 3 and longer than 45 characters.")]             
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Email address  is required.")]      
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resource))]
        [System.Web.Mvc.Remote("IsEmailUnique", "User", ErrorMessage = "Email already exists try another.")]
        public string email_address { get; set; }

        [Required(ErrorMessage = "Password  is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Password name cannot be smaller than 3 and longer than 45 characters.")]                  
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string password { get; set; }
        [Display(Name = "BacktoUsers", ResourceType = typeof(Resource))]
        public string back_to_users { get; set; }
        [Display(Name = "CreateUser",ResourceType=typeof(Resource))]
        public string create_user { get; set; }
        [Display(Name = "DashboardCreateUser", ResourceType = typeof(Resource))]
        public string dashboard_create_user { get; set; }
        [Display(Name = "Create",ResourceType=typeof(Resource))]
        public string create { get; set; }

        [Display(Name = "User", ResourceType = typeof(Resource))]
        public string user { get; set; }

    }

    public class UpdateUserViewModel
    {
        [MaxLength(128, ErrorMessage = "user id cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "user id cannot be smaller than 3 characters.")]
        public string user_id { get; set; }

        [Required(ErrorMessage = "User name  is required.")]
        [MaxLength(45, ErrorMessage = "User name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "User name cannot be smaller than 3 characters.")]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        [Required(ErrorMessage = "First name  is required.")]
        [MaxLength(45, ErrorMessage = "First name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "First name cannot be smaller than 3 characters.")]
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(45, ErrorMessage = "Last name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Last name cannot be smaller than 3 characters.")]
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Email address  is required.")]
        [MaxLength(45, ErrorMessage = "Email address cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Email address cannot be smaller than 3 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resource))]
        public string email_address { get; set; }

        //[Required(ErrorMessage = "country code  is required.")]
        public string country_code { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        //[MaxLength(10, ErrorMessage = "Phone number cannot be longer than 10 characters.")]
        //[MinLength(10, ErrorMessage = "Phone number cannot be smaller than 10 characters.")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resource))]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "Address  is required.")]
        [MaxLength(150, ErrorMessage = "Address cannot be longer than 150 characters.")]
        [MinLength(3, ErrorMessage = "Address cannot be smaller than 3 characters.")]
        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string address { get; set; }

        [MaxLength(150, ErrorMessage = "Alternate address cannot be longer than 150 characters.")]
        [MinLength(3, ErrorMessage = "Alternate address cannot be smaller than 3 characters.")]
        [Display(Name = "AlternateAddress", ResourceType = typeof(Resource))]
        public string alternate_address { get; set; }

        
        [ForeignKey("CountryMaster")]
        public int? country_id { get; set; }

        [Required(ErrorMessage = "City  is required.")]
        [MaxLength(45, ErrorMessage = "City cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "City cannot be smaller than 3 characters.")]
        [Display(Name = "City", ResourceType = typeof(Resource))]
        public string city { get; set; }

        [Required(ErrorMessage = "Zip  is required.")]
        [MaxLength(12, ErrorMessage = "Zip cannot be longer than 12 characters.")]
        [MinLength(3, ErrorMessage = "Zip cannot be smaller than 3 characters.")]
        [Display(Name = "Zip", ResourceType = typeof(Resource))]
        public string zip { get; set; }

        //[Required(ErrorMessage = "Password  is required.")]
        //[MaxLength(45, ErrorMessage = "password cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "password cannot be smaller than 3 characters.")]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string password { get; set; }

        [Required(ErrorMessage = "user statusid  is required.")]
        [ForeignKey("MemberStatusMaster")]
        public int user_statusid { get; set; }

        [Required(ErrorMessage = "company id  is required.")]
        public int company_id { get; set; }

        public List<Wollo.Entities.ViewModels.country_master> CountryDetails { get; set; }
        public UpdateUserViewModel()
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

    }


    public class RoleViewModel : BaseAuditableViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        [RegularExpression(@"^[a-zA-Z_ ]+$", ErrorMessage = "Invalid Role Name.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        //***************************************** Label Properties ************************************************//
        [Display(Name = "CreateRole", ResourceType = typeof(Resource))]
        public string create_role { get; set; }
        [Display(Name = "DashboardCreateRole",ResourceType=typeof(Resource))]
        public string dashboard_create_role { get; set; }
        [Display(Name = "Create", ResourceType = typeof(Resource))]
        public string create { get; set; }
        [Display(Name = "Reset", ResourceType = typeof(Resource))]
        public string reset { get; set; }
        [Display(Name = "BacktoRoles", ResourceType = typeof(Resource))]
        public string back_to_roles { get; set; }
        [Display(Name = "RoleName", ResourceType = typeof(Resource))]
        public string role_name { get; set; }
    }
}
