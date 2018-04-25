using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Issue_Cash_Transfer_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [ForeignKey("IssuerUser")]
        public string issuer_account_id { get; set; }
        [DataMember]
        [ForeignKey("ReceiverUser")]
        public string receiver_account_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued on date is required.")]
       // [Display(Name="DateTime",ResourceType=typeof(Resource))]
        public DateTime cash_issued_on_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issue permission id is required.")]
        [ForeignKey("IssueWithdrawelPermissionMaster")]
        public int cash_issue_permission_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued is required.")]
        //[Display(Name = "Amount", ResourceType = typeof(Resource))]
        public float cash_issued { get; set; }
        [DataMember]
        public bool is_admin { get; set; }

        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; } 
        [DataMember]
        public virtual Issue_Withdrawel_Permission_Master IssueWithdrawelPermissionMaster { get; set; }
        [DataMember]
        public virtual AspnetUsers IssuerUser { get; set; }
        [DataMember]
        public virtual AspnetUsers ReceiverUser { get; set; }

        //****************************** Label Properties ***********************************************//
        [Display(Name = "RequestForCash", ResourceType = typeof(Resource))]
        public string request_for_cash { get; set; }

        [Display(Name = "CashRequest", ResourceType = typeof(Resource))]
        public string cash_request { get; set; }

        [Display(Name = "DashboardCashRequest", ResourceType = typeof(Resource))]
        public string dashboard_cash_request { get; set; }

        [Display(Name = "CashRequestHistory", ResourceType = typeof(Resource))]
        public string cash_request_history { get; set; }
        [Display(Name = "RequestCash", ResourceType = typeof(Resource))]
        public string request_cash { get; set; }

        [Display(Name="Actions",ResourceType=typeof(Resource))]
        public string actions { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [Display(Name = "CashIssueRequest", ResourceType = typeof(Resource))]
        public string cash_issue_request { get; set; }

        [Display(Name = "DashboardCashIssueRequest", ResourceType = typeof(Resource))]
        public string dashboard_cash_issue_request { get; set; }

        [Display(Name = "CashIssuedHistory", ResourceType = typeof(Resource))]
        public string cash_issued_history { get; set; }

        [Display(Name = "AdminName", ResourceType = typeof(Resource))]
        public string admin_name { get; set; }

        [Display(Name = "ChangeStatus", ResourceType = typeof(Resource))]
        public string change_status { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string amount { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public string date_time { get; set; }

        [Display(Name = "CashRequestApproval", ResourceType = typeof(Resource))]
        public string cash_request_approval { get; set; }

        [Display(Name = "CashIssuedIntoSystemHistory", ResourceType = typeof(Resource))]
        public string cash_issued_into_system_history { get; set; }

        [Display(Name = "DashboardCashIssuedIntoSystemHistory", ResourceType = typeof(Resource))]
        public string dashboard_cash_issued_into_system_history { get; set; }

        [Display(Name = "ApprovedBy", ResourceType = typeof(Resource))]
        public string approved_by { get; set; }

        [Display(Name = "CashIssuedToMembersHistory", ResourceType = typeof(Resource))]
        public string cash_issued_to_members_history { get; set; }

        [Display(Name = "DashboardCashIssuedToMembersHistory", ResourceType = typeof(Resource))]
        public string dashboard_cash_issued_to_members_history { get; set; }
      
        //****************************** Label Properties ***********************************************//
    }
}
