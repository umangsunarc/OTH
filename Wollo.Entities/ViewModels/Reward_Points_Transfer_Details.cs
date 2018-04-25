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
    public class Reward_Points_Transfer_Details
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [ForeignKey("RewardPointTransferMaster")]
        public int pointstransfer_id { get; set; }
        [ForeignKey("TransferActionMaster")]
        [DataMember]
        public int transfer_actionid { get; set; }
        [DataMember]
        public int points_transferred { get; set; }
        [DataMember]
        public virtual Transfer_Action_Master TransferActionMaster { get; set; }
        [DataMember]
        public virtual Reward_Points_Transfer_Master RewardPointTransferMaster { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        //************************Label*************************//
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public int amount { get; set; }

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public DateTime date_time { get; set; }

        [Display(Name = "TransferredFrom", ResourceType = typeof(Resource))]
        public string transfered_from { get; set; }
        [Display(Name = "TransferredAmount", ResourceType = typeof(Resource))]
        public string transfered_amount { get; set; }

        [Display(Name = "Rewardpointstransferhistory", ResourceType = typeof(Resource))]
        public string reward_points_transfer_history { get; set; }

        [Display(Name = "DashboardRewardPointsTransferHistory", ResourceType = typeof(Resource))]
        public string dashboard_reward_points_transfer_history { get; set; }
        //************************End***************************//
    }
}
