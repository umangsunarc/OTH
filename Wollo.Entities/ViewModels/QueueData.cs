using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class QueueData
    {

        [DataMember]
        public float highestBidRate { get; set; }
        [DataMember]
        public float LowestAskRate { get; set; }
        [DataMember]
        public float bidMemberPercent { get; set; }
        [DataMember]
        public float askMemberPercent { get; set; }
        [DataMember]
        [Display(Name = "LastTradedPrice", ResourceType = typeof(Resource))]
        public float lastTradedPrice { get; set; }
        [DataMember]
        public string lastTradedTime { get; set; }
        [DataMember]
        public float lastTradedPriceDifference { get; set; }
        [DataMember]
        public float lastTradedPricePercentDifference { get; set; }
        [DataMember]
        [Display(Name = "DayHigh", ResourceType = typeof(Resource))]
        public float dayHigh { get; set; } //Highest traded rate of today
        [DataMember]
        [Display(Name = "DayLow", ResourceType = typeof(Resource))]
        public float dayLow { get; set; } //Lowest traded rate of today
        [DataMember]
        [Display(Name = "PrevClose", ResourceType = typeof(Resource))]
        public float prevClose { get; set; } //Last traded rate of yesterday
        [DataMember]
        [Display(Name = "TotatlVolume", ResourceType = typeof(Resource))]
        public int totatlVolume { get; set; } //Total volume of today(traded+queuing)
        [DataMember]
        [Display(Name = "Open", ResourceType = typeof(Resource))]
        public float open { get; set; } //First trading of day
        [DataMember]
        [Display(Name = "NoOfTraded", ResourceType = typeof(Resource))]
        public int noOfTraded { get; set; }  //Traded volume of today
        [DataMember]
        [Display(Name = "LotSize", ResourceType = typeof(Resource))]
        public int lotSize { get; set; }
        [DataMember]
        [Display(Name = "ValueTraded", ResourceType = typeof(Resource))]
        public float valueTraded { get; set; }
        [DataMember]
        [Display(Name = "OneYearMinMaxRate", ResourceType = typeof(Resource))]
        public string oneYearMinMaxRate { get; set; }

    }
}
