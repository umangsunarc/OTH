using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
     [DataContract]
     public class Cash_Transaction_Detail : Entity
    {
        [DataMember]
        [ForeignKey("IssuerUsers")]
        public string issuer_account_id { get; set; }

        [DataMember]
        [ForeignKey("ReceiverUser")]
        public string receiver_account_id { get; set; }

        [DataMember]
        public DateTime cash_issued_on_date { get; set; }

        [DataMember]
        public float opening_cash { get; set; }

        [DataMember]
        public float transaction_amount { get; set; }

        [DataMember]
        public float closing_cash { get; set; }

        [DataMember]
        public string description { get; set; }
         [DataMember]
        public virtual AspnetUsers IssuerUsers { get; set; }
          [DataMember]
        public virtual AspnetUsers ReceiverUser { get; set; }
    }
}
