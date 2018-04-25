using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;


namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class PortfolioDetails
    {
         //[DataMember]
         //public List<Wollo.Entities.ViewModels.Stock_Code> StockCode { get; set; }

         //[DataMember]
         //public List<Wollo.Entities.ViewModels.Transaction_History> TransactionHistory { get; set; }

         //[DataMember]
         //public List<Wollo.Entities.ViewModels.Member_Stock_Details> MemberStockDetails { get; set; }
         //public PortfolioDetails()
         //{
         //    StockCode = new List<Wollo.Entities.ViewModels.Stock_Code>();
         //    TransactionHistory = new List<Wollo.Entities.ViewModels.Transaction_History>();
         //    MemberStockDetails = new List<Wollo.Entities.ViewModels.Member_Stock_Details>();
         //}
        [DataMember]
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public string Cash { get; set; }

        [DataMember]
        [Display(Name = "Total", ResourceType = typeof(Resource))]
        public string Total { get; set; }
        [DataMember]
        public float HoldingCash { get; set; }
        [DataMember]
        public float BuyableSellableCash { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString="{0:n2}",ApplyFormatInEditMode=true)]
        public float LastTransactedPriceCash { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float LatestMarketvalueCash { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float LatestMarketvalueCashTotal { get; set; }
        [DataMember]
        public int StockId { get; set; }

        [DataMember]
        public string StockCode { get;set; }
        [DataMember]
        public string StockName { get; set; }
        [DataMember]
        public int Holdings { get; set; }
        [DataMember]
        public int BuyableSellableQuantity { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float LastTransactedPrice { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float LatestMarketvalue { get; set; }

        //*****************************************Label Enteries*************************************/
        [Display(Name = "DashboardPortfolio", ResourceType = typeof(Resource))]
        public string dashboard_portfolio { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Resource))]
        public string toatl { get; set; }

        [Display(Name = "Portfolio", ResourceType = typeof(Resource))]
        public string portfolio { get; set; }
        [Display(Name = "StockName", ResourceType = typeof(Resource))]
        public string stock_name { get; set; }
        [Display(Name = "StockCode", ResourceType = typeof(Resource))]
        public string stock_code { get; set; }
        [Display(Name = "Holdings", ResourceType = typeof(Resource))]
        public string holdings_amount { get; set; }
        [Display(Name = "BuyableSellableQuantity", ResourceType = typeof(Resource))]
        public string buyable_sellable_quantity { get; set; }
        [Display(Name = "LastTransactedPrice", ResourceType = typeof(Resource))]
        public string last_transacted_price { get; set; }
        [Display(Name = "LatestMarketValue", ResourceType = typeof(Resource))]
        public string lates_market_value { get; set; }
    }
}








