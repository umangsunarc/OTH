using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    public class PaymentSuccess
    {
        public string Status { get; set; }
        public string userEmail { get; set; }
        public string PaymentType { get; set; }
        public string Product { get; set; }
        public string Amount { get; set; }
        public string transactionId { get; set; }
    }

    public class PaymentFailure
    {
        public string Status { get; set; }
        public string userEmail { get; set; }
        public string PaymentType { get; set; }
        public string Product { get; set; }
        public string Amount { get; set; }
    }
}