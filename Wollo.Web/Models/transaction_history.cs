using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Wollo.Web.Models;

namespace Wollo.Web.Models
{
    public class transaction_history
    {
        public string user_id { get; set; }

        public int reward_points { get; set; }

        public int price { get; set; }

        public string queue_action { get; set; }

        public float rate { get; set; }

        [ForeignKey("QueueStatusMaster")]
        public int status_id { get; set; }

        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }

        public virtual Stock_Code StockCode { get; set; }

        public virtual Queue_Status_Master QueueStatusMaster { get; set; }
    }
}