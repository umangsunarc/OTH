using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    class Permissions_Master : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "description is required.")]
        [MaxLength(45, ErrorMessage = "description cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "description cannot be smaller than 3 characters.")]
        public string description { get; set; }
    }
}
