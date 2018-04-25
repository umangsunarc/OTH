using System.Runtime.Serialization;
using Wollo.Base.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wollo.Entities.Models
{
    [DataContract]
    //[TrackChanges]
    public class Reward_Points_Transfer_Details : Entity
    {
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
    }
}
