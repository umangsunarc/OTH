using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Cash_Transaction_Detail
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [ForeignKey("IssuerUsers")]
        public string issuer_account_id { get; set; }

        [DataMember]
        [ForeignKey("ReceiverUser")]
        public string receiver_account_id { get; set; }

        [DataMember]
        public DateTime cash_issued_on_date { get; set; }

        [DataMember]
        [Display(Name = "OpeningCash", ResourceType = typeof(Resource))]
        public float opening_cash { get; set; }

        [DataMember]
        [Display(Name = "TransactionAmount", ResourceType = typeof(Resource))]
       public float transaction_amount { get; set; }

        [DataMember]
        [Display(Name = "ClosingCash", ResourceType = typeof(Resource))]
        public float closing_cash { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }

        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        [DataMember]
        public virtual AspnetUsers IssuerUsers { get; set; }
        [DataMember]
        public virtual AspnetUsers ReceiverUser { get; set; }

        //************************************Label for properties************************************//

      
        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public DateTime date_time { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resource))]
        public float currency { get; set; }

        [Display(Name = "Filter", ResourceType = typeof(Resource))]
        public float filter { get; set; }

        [Display(Name = "Refresh", ResourceType = typeof(Resource))]
        public float refresh { get; set; }

        [Display(Name = "From", ResourceType = typeof(Resource))]
        public float from { get; set; }

        [Display(Name = "To", ResourceType = typeof(Resource))]
        public float to { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public float amount { get; set; }

        [Display(Name = "CashHistory", ResourceType = typeof(Resource))]
        public string cash_history { get; set; }

        [Display(Name = "Issuer", ResourceType = typeof(Resource))]
        public string issuer { get; set; }

        [Display(Name = "Receiver", ResourceType = typeof(Resource))]
        public string receiver { get; set; }

        [Display(Name = "SystemCashDetails", ResourceType = typeof(Resource))]
        public string system_cash_details { get; set; }

        //[Display(Name = "OpeningCash", ResourceType = typeof(Resource))]
        //public string opening_cash { get; set; }

        //[Display(Name = "ClosingCash", ResourceType = typeof(Resource))]
        //public string closing_cash { get; set; }

        //***********************************Label for properties************************************//

    }
}
