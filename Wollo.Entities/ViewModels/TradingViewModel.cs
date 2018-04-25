using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    public class TradingViewModel
    {
        public List<Transaction_History> Transaction_History { get; set; }
        public List<Stock_Code> Stock_Code { get; set; }
        public QueueData queue_data { get; set; }
        public Units_Master unit_master { get; set; }

        //******************************************* Label Properties *********************************************//
        [Display(Name="Trading",ResourceType=typeof(Resource))]
        public string trading { get; set; }
        [Display(Name = "DashboardTrading", ResourceType = typeof(Resource))]
        public string dashboard_trading { get; set; }
        [Display(Name = "BuyorSell", ResourceType = typeof(Resource))]
        public string buy_or_sell { get; set; }
        [Display(Name = "Bid", ResourceType = typeof(Resource))]
        public string bid { get; set; }
        [Display(Name = "Ask", ResourceType = typeof(Resource))]
        public string ask { get; set; }
        [Display(Name = "Iwanttobuy", ResourceType = typeof(Resource))]
        public string i_want_to_buy { get; set; }
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
        //******************************************** Label Properties **********************************************//

        public TradingViewModel()
        {
            Transaction_History = new List<Transaction_History>();
            Stock_Code = new List<Stock_Code>();
            queue_data = new QueueData();
            unit_master = new Units_Master();
        }
    }
}
