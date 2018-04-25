using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Entities.ViewModels;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    public class QueueTradingViewModel
    {
        //************************************ TradingViewModel ********************************************************//
        public List<Transaction_History> Transaction_History { get; set; }
        public List<Stock_Code> Stock_Code { get; set; }
        public QueueData queue_data { get; set; }
        public Units_Master unit_master { get; set; }
        //************************************ TradingViewModel ********************************************************//
        
        //****************************************** Queue Data View Model *********************************************//
        public List<Traded_History_Master> TradedHistory { get; set; }
        [DataMember]
        [Display(Name = "Bid", ResourceType = typeof(Resource))]
        public List<Transaction_History> Bid { get; set; }

        [DataMember]
        [Display(Name = "Ask", ResourceType = typeof(Resource))]
        public List<Transaction_History> Ask { get; set; }
        public QueueData QueueData { get; set; }
        public List<Stock_Code> StockCode { get; set; }
        //****************************************** Queue Data View Model *********************************************//

        //*****************************************Label Enteries*************************************/
        [DataMember]
        [Display(Name = "Queue", ResourceType = typeof(Resource))]
        public string queue { get; set; }

        [DataMember]
        [Display(Name = "DashboardQueue", ResourceType = typeof(Resource))]
        public string dashboard_queue { get; set; }

        [DataMember]
        [Display(Name = "InQueuing", ResourceType = typeof(Resource))]
        public string in_queue { get; set; }

        [DataMember]
        [Display(Name = "Ask", ResourceType = typeof(Resource))]
        public string ask { get; set; }

        [DataMember]
        [Display(Name = "Price", ResourceType = typeof(Resource))]
        public string price { get; set; }

        [DataMember]
        [Display(Name = "Quantity", ResourceType = typeof(Resource))]
        public string quantity { get; set; }

        [DataMember]
        [Display(Name = "NumberofUsers", ResourceType = typeof(Resource))]
        public string number_of_users { get; set; }

        [DataMember]
        [Display(Name = "Traded", ResourceType = typeof(Resource))]
        public string traded { get; set; }

        [DataMember]
        [Display(Name = "Timestamp", ResourceType = typeof(Resource))]
        public string timestamp { get; set; }

        [Display(Name = "Trading", ResourceType = typeof(Resource))]
        public string trading { get; set; }

        [Display(Name = "DashboardTrading", ResourceType = typeof(Resource))]
        public string dashboard_trading { get; set; }

        [Display(Name = "BuyorSell", ResourceType = typeof(Resource))]
        public string buy_or_sell { get; set; }

        [Display(Name = "Iwanttobuy", ResourceType = typeof(Resource))]
        public string i_want_to_buy { get; set; }

        [DataMember]
        [Display(Name = "BidPrice", ResourceType = typeof(Resource))]
        public float bid_price { get; set; }

        //[Display(Name = "MyOrders", ResourceType = typeof(Resource))]
        //public string my_orders { get; set; }

        //[Display(Name = "Trading", ResourceType = typeof(Resource))]
        //public string i_want_to_sell { get; set; }

        [Display(Name = "ForaTotalOf", ResourceType = typeof(Resource))]
        public string for_a_total_of { get; set; }

        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string actions { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Modify", ResourceType = typeof(Resource))]
        public string modify { get; set; }

        [DataMember]
        [Display(Name = "Volume", ResourceType = typeof(Resource))]
        public string volume { get; set; }

        [DataMember]
        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string trading_time { get; set; }

        [DataMember]
        [Display(Name = "LastTraded", ResourceType = typeof(Resource))]
        public string last_traded { get; set; }

        [DataMember]
        [Display(Name = "ValueTraded", ResourceType = typeof(Resource))]
        public string value_traded { get; set; }

        [DataMember]
        [Display(Name = "TotalVolume", ResourceType = typeof(Resource))]
        public string totatl_volume { get; set; }

        [DataMember]
        [Display(Name = "MyOrders", ResourceType = typeof(Resource))]
        public string my_orders { get; set; }

        [DataMember]
        [Display(Name = "OneDay", ResourceType = typeof(Resource))]
        public string one_day { get; set; }

        [DataMember]
        [Display(Name = "FiveDay", ResourceType = typeof(Resource))]
        public string five_day { get; set; }

        [DataMember]
        [Display(Name = "OneMonth", ResourceType = typeof(Resource))]
        public string one_month { get; set; }

        [DataMember]
        [Display(Name = "OneYear", ResourceType = typeof(Resource))]
        public string one_year { get; set; }

        [DataMember]
        [Display(Name = "SixMonth", ResourceType = typeof(Resource))]
        public string six_month { get; set; }

        [DataMember]
        [Display(Name = "FiveYear", ResourceType = typeof(Resource))]
        public string five_year { get; set; }

        [DataMember]
        [Display(Name = "TotalTime", ResourceType = typeof(Resource))]
        public string total_time { get; set; }

        [DataMember]
        [Display(Name = "Weeks", ResourceType = typeof(Resource))]
        public string fiftytwo_Weeks { get; set; }

        [DataMember]
        [Display(Name = "SelectStock", ResourceType = typeof(Resource))]
        public string select_stock { get; set; }

        [DataMember]
        [Display(Name = "Forapriceof", ResourceType = typeof(Resource))]
        public string for_a_price_of { get; set; }

        [DataMember]
        [Display(Name = "sell", ResourceType = typeof(Resource))]
        public string sell { get; set; }

        [DataMember]
        [Display(Name = "Bid", ResourceType = typeof(Resource))]
        public string bid_property { get; set; }

        [DataMember]
        [Display(Name = "Ask", ResourceType = typeof(Resource))]
        public string ask_property { get; set; }

        [DataMember]
        [Display(Name = "PriceperLot", ResourceType = typeof(Resource))]
        public string price_per_Lot { get; set; }

        [DataMember]
        [Display(Name = "TotalPoints", ResourceType = typeof(Resource))]
        public string total_points { get; set; }

        [DataMember]
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string amount { get; set; }

        //********************************* label properties end***********************************//

        public QueueTradingViewModel()
        {
            Transaction_History = new List<Transaction_History>();
            Stock_Code = new List<Stock_Code>();
            queue_data = new QueueData();
            unit_master = new Units_Master();

            TradedHistory = new List<Traded_History_Master>();
            Bid = new List<Transaction_History>();
            Ask = new List<Transaction_History>();
            QueueData = new QueueData();
            StockCode = new List<Stock_Code>();
        }
    }
}
