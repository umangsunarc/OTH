using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Wollo.Base.LocalResource;
using System;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Topup_History : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public int amount { get; set; }
        [DataMember]
        [ForeignKey("TopupStatusMaster")]
        public int topup_status_id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Details cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Details cannot be smaller than 3 characters.")]
        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Payment Method cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Payment Method cannot be smaller than 3 characters.")]
        [Display(Name = "PaymentMethod", ResourceType = typeof(Resource))]
        public string payment_method { get; set; }
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; } 
        [DataMember]
        public virtual Topup_Status_Master TopupStatusMaster { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }

        //***********************Label for properties****************************//

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public DateTime date_time { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "TopupByMembersHistory", ResourceType = typeof(Resource))]
        public string topup_by_members_history { get; set; }

        [Display(Name = "DashboardTopupByMembersHistory", ResourceType = typeof(Resource))]
        public string dashboard_topup_by_members_history { get; set; }

        //******************************end*************************************//
    }
}
