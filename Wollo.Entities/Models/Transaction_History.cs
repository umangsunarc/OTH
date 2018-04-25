using Wollo.Base.Entity;
using Wollo.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Transaction_History : AuditableEntity
    {

        [DataMember]
        public string user_id { get; set; }

        [DataMember]
        public int reward_points { get; set; }

        [DataMember]
        public float price { get; set; }

        [DataMember]
        [MaxLength(10, ErrorMessage = "Queue action cannot be longer than 10 characters.")]
        [Required(ErrorMessage="Queue action is required.")]
        public string queue_action { get; set; }

        [DataMember]
        public float rate { get; set; }
        [DataMember]
        public int original_order_quantity { get; set; }

        [DataMember]
        [ForeignKey("QueueStatusMaster")]
        public int status_id { get; set; }

        [DataMember]
        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }

        [DataMember]
        public virtual Queue_Status_Master QueueStatusMaster { get; set; }
    }
}
