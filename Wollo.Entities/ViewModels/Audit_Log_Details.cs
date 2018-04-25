using Wollo.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    class Audit_Log_Details
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "property name is required.")]
        [MaxLength(45, ErrorMessage = "property name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "property name cannot be smaller than 3 characters.")]
        public string property_name { get; set; }

        [DataMember]
        [Required(ErrorMessage = "original value is required.")]
        [MaxLength(45, ErrorMessage = "original value cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "original value cannot be smaller than 3 characters.")]
        public string original_value { get; set; }

        [DataMember]
        [Required(ErrorMessage = "new value is required.")]
        [MaxLength(45, ErrorMessage = "new value cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "new value cannot be smaller than 3 characters.")]
        public string new_value { get; set; }

        [DataMember]
        [Required(ErrorMessage = "audit id is required.")]
        [ForeignKey("AuditLogMaster")]
        public int auditlog_id { get; set; }
        [DataMember]
        public virtual AuditLog AuditLogMaster { get; set; }
    }
}
