using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Withdrawl_Status_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "status is required.")]
        [MaxLength(45, ErrorMessage = "status cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "status  cannot be smaller than 3 characters.")]
        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }
    }
}
