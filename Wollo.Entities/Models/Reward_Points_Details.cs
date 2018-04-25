using System.Runtime.Serialization;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    //[TrackChanges]
    public class Reward_Points_Details : AuditableEntity
    {
        [DataMember]
        public string user_id { get; set; }
        [DataMember]
        public int points { get; set; }

        [DataMember]
        public int account_id { get; set; }
    }
}
