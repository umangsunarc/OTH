using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Rule_Config_Settings : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Rule id is required.")]
        [ForeignKey("RuleConfigMaster")]
        public int rule_id { get; set; }

        [DataMember]
        [MaxLength(100, ErrorMessage = "description cannot be longer than 100 characters.")]
        [MinLength(3, ErrorMessage = "description cannot be smaller than 3 characters.")]
        [Required(ErrorMessage = "description is required.")]
        public string description { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "rule_type cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "rule_type cannot be smaller than 3 characters.")]

        public string rule_type { get; set; }
        [DataMember]
        public Nullable<int> rule_order { get; set; }

        [DataMember]
        public virtual Rule_Config_Master RuleConfigMaster { get; set; }
    }
}
