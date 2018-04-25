using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Topup_Status_Master : AuditableEntity
    {
        [DataMember]
        [MaxLength(45, ErrorMessage = "Status cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Status cannot be smaller than 3 characters.")]
        public string status { get; set; }
    }
}
