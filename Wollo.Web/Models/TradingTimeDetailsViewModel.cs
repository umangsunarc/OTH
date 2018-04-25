using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wollo.Web.Helper;
using Wollo.Base.LocalResource;
using Wollo.Base.Entity;

namespace Wollo.Web.Models
{
    public class TradingTimeDetailsViewModel : BaseAuditableViewModel
    {
        public int id { get; set; }

        //public int[] selected_days { get; set; }

        public TimeSpan? start_time { get; set; }

        [Required(ErrorMessage = "end time is required.")]

        public TimeSpan? end_time { get; set; }

        //public Wollo.Entities.ViewModels.WeakDays[] Days { get; set; }
        public List<Wollo.Entities.ViewModels.WeakDays> Days { get; set; }


        //****************************** Label Properties ***********************************************//
        [Display(Name = "DashboardTradingTime", ResourceType = typeof(Resource))]
        public string dashboard_trading_time { get; set; }

        [Display(Name = "AdminSettings", ResourceType = typeof(Resource))]
        public  string admin_settings { get; set; }

        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string trading_time { get; set; }

        [Display(Name = "StartingTime", ResourceType = typeof(Resource))]
        public string starting_time { get; set; }

        [Display(Name = "CutOffTime", ResourceType = typeof(Resource))]
        public string cut_off_time { get; set; }

        [Display(Name = "TradingDay", ResourceType = typeof(Resource))]
        public string trading_day { get; set; }

        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }

        //****************************** Label Properties ***********************************************//
    }

    
}