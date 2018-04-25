using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Traded_History_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [MaxLength(10, ErrorMessage = "Queue action cannot be longer than 10 characters.")]
        [Required(ErrorMessage = "Queue action is required.")]
        [Display(Name="BidAsk",ResourceType=typeof(Resource))]
        public string queue_action { get; set; }

        [DataMember]
        [Display(Name = "QTY", ResourceType = typeof(Resource))]
        public int amount { get; set; }

        [DataMember]
        [Display(Name = "TradedBidPrice", ResourceType = typeof(Resource))]
        public float bid_price { get; set; }

        [DataMember]
        [Display(Name = "AskPrice", ResourceType = typeof(Resource))]
        public float ask_price { get; set; }

        [DataMember]
        [ForeignKey("StockCode")]
        public int stock { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
    }
}
