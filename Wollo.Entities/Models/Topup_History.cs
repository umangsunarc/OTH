using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Topup_History : AuditableEntity
    {
        [DataMember]
        public int amount { get; set; }
        [DataMember]
        [ForeignKey("TopupStatusMaster")]
        public int topup_status_id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Details cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Details cannot be smaller than 3 characters.")]
        public string details { get; set; }
        [DataMember]
        //[Display(Name="Payment Method")]
        [MaxLength(45, ErrorMessage = "Payment Method cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "Payment Method cannot be smaller than 3 characters.")]
        public string payment_method { get; set; }

        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public virtual Topup_Status_Master TopupStatusMaster { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
    }
}
