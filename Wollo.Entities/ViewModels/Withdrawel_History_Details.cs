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
    public class Withdrawel_History_Details
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "withdrawer user id  is required.")]
        [ForeignKey("AspnetUsers")]
        public string withdrawer_user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "withdrawer account id is required.")]

        public int withdrawer_account_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "amount  is required.")]
        [Display(Name="Amount",ResourceType=typeof(Resource))]
        public float amount { get; set; }

        [DataMember]
        [Required(ErrorMessage = "withdrawel history id is required.")]
        [ForeignKey("WithdrawelHistoryMaster")]
        public int withdrawel_history_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "withdrawel permission id  is required.")]
        [ForeignKey("IssueWithdrawelPermissionMaster")]
        public int withdrawel_permission_id { get; set; }

        [DataMember]
        [ForeignKey("WithdrawlStatusMaster")]
        public int withdrawel_status_id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "first name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "first name cannot be smaller than 3 characters.")]
        public string payment_method { get; set; }

        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; } 

        [DataMember]
        public virtual Withdrawel_History_Master WithdrawelHistoryMaster { get; set; }

        [DataMember]
        public virtual Issue_Withdrawel_Permission_Master IssueWithdrawelPermissionMaster { get; set; }

        [DataMember]
        public virtual Withdrawl_Status_Master WithdrawlStatusMaster { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }

        //*****************************Label For Properties*************************//

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public DateTime date_time { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "WithdrawOutByMembersHistory", ResourceType = typeof(Resource))]
        public string withdraw_out_by_members_history { get; set; }

        [Display(Name = "DashboardWithdrawOutByMembersHistory", ResourceType = typeof(Resource))]
        public string dashboard_withdraw_out_by_members_history { get; set; }

        //*****************************End*****************************************//
    }
}
