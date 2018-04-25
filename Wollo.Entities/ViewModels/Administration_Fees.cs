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
    public class Administration_Fees: BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }


        [DataMember]
        [ForeignKey("RuleConfigMaster")]
        public int rule_id { get; set; }
        [DataMember]
        public float fees { get; set; }
        [DataMember]
        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }
        [DataMember]
        public string apply_on { get; set; }
        [DataMember]
        public virtual Rule_Config_Master RuleConfigMaster { get; set; }
        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
    }
}
