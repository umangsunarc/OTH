using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    public class AddTopupModel
    {
        public string name { get; set; }
        public string appid { get; set; }
        public int aes_id { get; set; }
        public int paypal_id { get; set; }
        public string user_email { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public string currency { get; set; }
        public string cancel_url { get; set; }
        public string return_url { get; set; }
        public int store_id { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public DateTime dob { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
    }
}
