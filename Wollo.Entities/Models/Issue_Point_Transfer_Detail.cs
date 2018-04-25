using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    
   public class Issue_Point_Transfer_Detail : Entity
    {
        [DataMember]
        [ForeignKey("IssuerUsers")]
        public string issuer_account_id { get; set; }

        [DataMember]
        [ForeignKey("ReceiverUser")]
        public string receiver_account_id { get; set; }

        [DataMember]
        public DateTime point_issued_on_date { get; set; }

        [DataMember]
        public int opening_amount { get; set; }

        [DataMember]
        public int transaction_amount { get; set; }

        [DataMember]
        public int closing_amount { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        [ForeignKey("StockId")]
        public int stock_id { get; set; }

        public virtual Stock_Code StockId { get; set; }
        public virtual AspnetUsers IssuerUsers { get; set; }
        public virtual AspnetUsers ReceiverUser { get; set; }
    }
}
