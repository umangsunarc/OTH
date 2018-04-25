using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Wollo.Base.LocalResource;
using System.ComponentModel;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Cash_Transaction_History : BaseAuditableViewModel
    {
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [ForeignKey("AspNetUsers")]
        public string user_id { get; set; }
        [DataMember]
        [Display(Name = "OpeningCashBalance", ResourceType = typeof(Resource))]
        public float opening_cash { get; set; }
        [DataMember]
        [Display(Name = "TransactionAmount", ResourceType = typeof(Resource))]
        public float transaction_amount { get; set; }
        [DataMember]
        [Display(Name = "ClosingBalance", ResourceType = typeof(Resource))]
        public float closing_cash { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Description cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Description cannot be smaller than 3 characters.")]
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }
        [DataMember]
        public virtual AspnetUsers AspNetUsers { get; set; }

        //****************************** Label Properties ***********************************************//
        [Display(Name = "IssueCash", ResourceType = typeof(Resource))]
        public string cash_issue { get; set; }

        [Display(Name = "CashTransactionHistory", ResourceType = typeof(Resource))]
        public string cash_transaction_history { get; set; }

        [Display(Name = "DashboardCashTransactionHistory", ResourceType = typeof(Resource))]
        public string dashboard_cash_transaction_history { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resource))]
        public string currency { get; set; }

        [Display(Name = "Payee", ResourceType = typeof(Resource))]
        public string payee { get; set; }

        [Display(Name = "MemberCashHistory", ResourceType = typeof(Resource))]
        public string member_cash_history { get; set; }

        [Display(Name = "DashboardCashMemberCash", ResourceType = typeof(Resource))]
        public string dashboard_cash_member_cash { get; set; }

        [Display(Name = "IssuedCashHistory", ResourceType = typeof(Resource))]
        public string issued_cash_history { get; set; }

        [Display(Name = "IssueCash", ResourceType = typeof(Resource))]
        public string issued_cash { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [Display(Name = "Filter", ResourceType = typeof(Resource))]
        public string filter { get; set; }

        [Display(Name = "Refresh", ResourceType = typeof(Resource))]
        public string refresh { get; set; }

        [Display(Name = "From", ResourceType = typeof(Resource))]
        public string from { get; set; }

        [Display(Name = "To", ResourceType = typeof(Resource))]
        public string to { get; set; }

        [Display(Name = "CashIssueHistory", ResourceType = typeof(Resource))]
        public string cash_issue_history { get; set; }

        
        //****************************** Label Properties ***********************************************//
    }
}
