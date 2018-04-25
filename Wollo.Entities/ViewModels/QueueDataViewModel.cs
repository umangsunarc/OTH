using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.ViewModels
{
    public class QueueDataViewModel
    {
        public List<Traded_History_Master> TradedHistory { get; set; }
        public List<Transaction_History> Bid { get; set; }
        public List<Transaction_History> Ask { get; set; }
        public QueueData QueueData { get; set; }
        public List<Stock_Code> StockCode { get; set; }
        public Units_Master UnitMaster { get; set; }

        //*****************************************Label Enteries*************************************/
        [DataMember]
        [Display(Name="Queue",ResourceType=typeof(Resource))]
        public string queue { get; set; }

        [DataMember]
        [Display(Name = "DashboardQueue", ResourceType = typeof(Resource))]
        public string dashboard_queue { get; set; }

        [DataMember]
        [Display(Name = "InQueuing", ResourceType = typeof(Resource))]
        public string in_queue { get; set; }

        [DataMember]
        [Display(Name = "Bid", ResourceType = typeof(Resource))]
        public string bid { get; set; }

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
        //********************************* label properties end***********************************//

        public QueueDataViewModel()
        {
            TradedHistory = new List<Traded_History_Master>();
            Bid = new List<Transaction_History>();
            Ask = new List<Transaction_History>();
            QueueData = new QueueData();
            StockCode = new List<Stock_Code>();
    }
    }
}
