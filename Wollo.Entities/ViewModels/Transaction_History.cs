using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Transaction_History : BaseAuditableViewModel
    {
        [DataMember]
        [Display(Name = "OrderID", ResourceType = typeof(Resource))]        
        public int id { get; set; }
        [DataMember]
        public string user_id { get; set; }

        [DataMember]
        [Display(Name = "RemainingOrderQTY", ResourceType = typeof(Resource))]     
        public int reward_points { get; set; }

        [DataMember]
        [Display(Name = "Price", ResourceType = typeof(Resource))]
        public float price { get; set; }

        [DataMember]
        [MaxLength(10, ErrorMessage = "Queue action cannot be longer than 10 characters.")]
        [Required(ErrorMessage = "Queue action is required.")]
        [Display(Name = "BidAsk", ResourceType = typeof(Resource))]
        public string queue_action { get; set; }

        [DataMember]
        public float rate { get; set; }
        [DataMember]
        [Display(Name = "OrderQTY", ResourceType = typeof(Resource))]
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

        //*****************************Properties for labels******************************************//
        //[DataMember]
        [Display(Name = "AllTradingHistory", ResourceType = typeof(Resource))]
        public string all_trading_history { get; set; }

        //[DataMember]
        [Display(Name = "DashboardAllTradingHistory", ResourceType = typeof(Resource))]
        public string dashboard_all_trading_history { get; set; }

        //[DataMember]
        [Display(Name = "TradingHistory", ResourceType = typeof(Resource))]
        public string trading_history { get; set; }

        //*****************************Properties for labels******************************************//
    }
}
