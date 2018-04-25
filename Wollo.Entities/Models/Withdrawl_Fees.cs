using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Withdrawl_Fees : AuditableEntity
    {
        [DataMember]
        [ForeignKey("RuleConfigMaster")]
        public int withdrawl_rule_id { get; set; }
        [DataMember]
        public float fees { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "first name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "first name cannot be smaller than 3 characters.")]
        public string method { get; set; }

        [DataMember]
        public virtual Rule_Config_Master RuleConfigMaster { get; set; }
    }
}
