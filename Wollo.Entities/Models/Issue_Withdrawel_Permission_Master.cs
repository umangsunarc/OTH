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
    public class Issue_Withdrawel_Permission_Master : AuditableEntity
    {
        [DataMember]

        [MaxLength(45, ErrorMessage = "permission cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "permission cannot be smaller than 3 characters.")]
        public string permission { get; set; }
    }
}
