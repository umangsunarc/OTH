using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    public class PointTransferModel
    {
        public string email { get; set; }
        public int stock_amount { get; set; }
        public string store_code { get; set; }
    }
}