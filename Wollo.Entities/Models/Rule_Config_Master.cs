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
    public class Rule_Config_Master : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "rule name is required.")]
        [MaxLength(200, ErrorMessage = "rule name cannot be longer than 200 characters.")]
        [MinLength(3, ErrorMessage = "rule name cannot be smaller than 3 characters.")]
        public string rule_name { get; set; }
    }
}
