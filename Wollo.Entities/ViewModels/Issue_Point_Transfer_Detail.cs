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
    public class Issue_Point_Transfer_Detail
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
       // [Display(Name = "DateTime", ResourceType = typeof(Resource))]

        public DateTime point_issued_on_date { get; set; }

        [DataMember]
        [Display(Name = "OpeningAmount", ResourceType = typeof(Resource))]

        public int opening_amount { get; set; }

        [DataMember]
        [Display(Name = "TransactionAmount", ResourceType = typeof(Resource))]
        public int transaction_amount { get; set; }

        [DataMember]
        [Display(Name = "ClosingAmount", ResourceType = typeof(Resource))]

        public int closing_amount { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }

        [DataMember]
        [ForeignKey("StockId")]
        public int stock_id { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; } 
        [DataMember]
        public virtual Stock_Code StockId { get; set; }
        [DataMember]
        public virtual AspnetUsers IssuerUsers { get; set; }
        [DataMember]
        public virtual AspnetUsers ReceiverUser { get; set; }

        //**************************************Label For Properties**********************************//

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public DateTime date_time { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public int amount { get; set; }

        [Display(Name = "IssuerUserName", ResourceType = typeof(Resource))]
        public string issuer_user_name { get; set; }

        [Display(Name = "ReceiverUserName", ResourceType = typeof(Resource))]
        public string receiver_username { get; set; }

        [Display(Name = "Issuer", ResourceType = typeof(Resource))]
        public string issuer { get; set; }

        [Display(Name = "Receiver", ResourceType = typeof(Resource))]
        public string receiver { get; set; }

        [Display(Name = "OpeningPoints", ResourceType = typeof(Resource))]
        public string opening_points { get; set; }

        [Display(Name = "ClosingPoints", ResourceType = typeof(Resource))]
        public string closing_points { get; set; }

        [Display(Name = "PointsIssued", ResourceType = typeof(Resource))]
        public string points_issued { get; set; }

        [Display(Name = "RewardPointHistory", ResourceType = typeof(Resource))]
        public string reward_point_history { get; set; }

        [Display(Name = "WolloRewardPointIssuedIntoSystemHistory", ResourceType = typeof(Resource))]
        public string wollo_reward_point_issued_into_system_History { get; set; }

        [Display(Name = "DashboardWolloRewardPointIssuedIntoSystemHistory", ResourceType = typeof(Resource))]
        public string dashboard_wollo_reward_point_issued_into_system_History { get; set; }

        [Display(Name = "WolloRewardPointIssuedToMemberHistory", ResourceType = typeof(Resource))]
        public string wollo_reward_point_issued_to_member_history { get; set; }

        [Display(Name = "DashboardWolloRewardPointIssuedToMemberHistory", ResourceType = typeof(Resource))]
        public string dashboard_wollo_reward_point_issued_to_member_history { get; set; }
       
       
        //**************************************End**************************************************//
    }
}
