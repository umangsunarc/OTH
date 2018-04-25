using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class IssuePointDetails
    {
        [DataMember]
        public int TotalPoints { get; set; }
        [DataMember]
        public int CancelledPoints { get; set; }
        [DataMember]
        public int RemainingPoints { get; set; }
        [DataMember]
        public virtual List<Issue_Points_Master> IssuePointsMaster { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }
        [DataMember]
        public virtual List<Issue_Points_Transfer_Master> IssuePointTransferMaster { get; set; }
        //**************************************** label properties ***************************************************//

        public string issued_points_history { get; set; }
        [Display(Name = "IssuedPointsValue", ResourceType = typeof(Resource))]
        public string issued_points_value { get; set; }
        [Display(Name = "IssueRewardPoints", ResourceType = typeof(Resource))]
        public string issue_reward_points { get; set; }
        [Display(Name = "TransferPoints", ResourceType = typeof(Resource))]
        public string transfer_points { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }

        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }

        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string actions { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }
        [Display(Name = "Stock", ResourceType = typeof(Resource))]
        public string stock { get; set; }

        [Display(Name = "IssuerUserName", ResourceType = typeof(Resource))]
        public string issuer_user_name { get; set; }

        [Display(Name = "ReceiverUserName", ResourceType = typeof(Resource))]
        public string receiver_user_name { get; set; }
        [Display(Name = "CurrentIssuedPoints", ResourceType = typeof(Resource))]
        public string Current_issued_points { get; set; }
        [Display(Name = "CancelledIssuedPoints", ResourceType = typeof(Resource))]
        public string Cancelled_issued_points { get; set; }
        [Display(Name = "Issuedpointbalance", ResourceType = typeof(Resource))]
        public string Issued_point_balance { get; set; }

        [Display(Name = "Modify", ResourceType = typeof(Resource))]
        public string modify { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "CancelledIssuedPoints", ResourceType = typeof(Resource))]
        public string cancelled_issued_points { get; set; }

        [Display(Name = "CurrentIssuedPoints", ResourceType = typeof(Resource))]
        public string current_issued_points { get; set; }

        [Display(Name = "Issuedpointbalance", ResourceType = typeof(Resource))]
        public string issued_point_balance { get; set; }
        //**************************************** label properties****************************************************//
    }
}
