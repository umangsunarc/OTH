using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Topup_Status_Master : BaseAuditableViewModel
    {
        [DataMember]
        [Display(Name = "ID", ResourceType = typeof(Resource))]

        public int id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Status cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Status cannot be smaller than 3 characters.")]
        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }
    }
}
