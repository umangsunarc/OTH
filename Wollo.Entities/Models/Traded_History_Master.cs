using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Wollo.Base.Entity;


namespace Wollo.Entities.Models
{
    [DataContract]
    public class Traded_History_Master : AuditableEntity
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [MaxLength(10, ErrorMessage = "Queue action cannot be longer than 10 characters.")]
        [Required(ErrorMessage = "Queue action is required.")]
        public string queue_action { get; set; }

        [DataMember]
        public int amount { get; set; }

        [DataMember]
        public float bid_price { get; set; }

        [DataMember]
        public float ask_price { get; set; }

        [DataMember]
        [ForeignKey("StockCode")]
        public int stock { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
    }
}
