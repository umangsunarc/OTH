using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class UpdateUserModel : BaseAuditableViewModel
    {
        public UpdateUserViewModel UpdateUserViewModel { get; set; }
        [Display(Name = "PersonalInfo", ResourceType = typeof(Resource))]
        public string personal_info { get; set; }
        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }
    }
}