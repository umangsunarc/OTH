using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.Models;
using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class MarketRateModel : BaseAuditableViewModel
    {
        public Market_Rate_Details MarketRateDetails { get; set; }
        //************************************ Label properties start **********************************//
        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string market_rate { get; set; }
        [Display(Name = "InitialMarketRate", ResourceType = typeof(Resource))]
        public string initial_market_rate { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }
        [Display(Name = "DashboardMarketRate", ResourceType = typeof(Resource))]
        public string dashboard_market_rate { get; set; }
        //********************************** End of properties ******************************************//
    }
}