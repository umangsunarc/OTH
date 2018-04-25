using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Market_Rate_Details : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Rate is required.")]
        public float rate { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        //************************************ Label properties start **********************************//
        [DataMember]
        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string market_rate { get; set; }
        [DataMember]
        [Display(Name = "InitialMarketRate", ResourceType = typeof(Resource))]
        public string initial_market_rate { get; set; }
        [DataMember]
        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }
        [DataMember]
        [Display(Name = "DashboardMarketRate", ResourceType = typeof(Resource))]
        public string dashboard_market_rate { get; set; }
        //********************************** End of properties ******************************************//
    }
}
