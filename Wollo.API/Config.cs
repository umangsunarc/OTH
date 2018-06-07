using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wollo.API
{
    public class Config
    {
        public bool supports_search { get; set; }
        public bool supports_group_request { get; set; }
        public bool supports_marks { get; set; }
        public Exchanges[] exchanges { get; set; }
        public SymbolsTypes[] symbolsTypes { get; set; }
        public string[] supportedResolutions { get; set; }        
    }
    public class Exchanges
    {
        public string value { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
    }
    public class SymbolsTypes
    {
        public string name { get; set; }   
        public string value { get; set; }
            
    }
    public class SymbolAPI
    {
        public string description { get; set; }
        public bool has_intraday { get; set; }
        public bool has_no_volume { get; set; }
        public int minmov { get; set; }
        public int minmov2 { get; set; }
        public string name { get; set; }
        public int pointvalue { get; set; }
        public int pricescale { get; set; }
        public string session { get; set; }
        public string[] supported_resolutions { get; set; }
        public string ticker { get; set; }
        public string timezone { get; set; }
        public string type { get; set; }
    }
    public class SymbolGroupAPI
    {
        public string[] symbol { get; set; }
        public string[] description { get; set; }

        [JsonProperty(PropertyName = "exchange-listed")]
        public string exchangeListed {get; set;}

        [JsonProperty(PropertyName = "exchange-traded")]
        public string exchangeTraded { get; set; }
        public int minmovement { get; set; }
        public int minmovement2 { get; set; }
        public int[] pricescale { get; set; }


        [JsonProperty(PropertyName = "has-dwm")]
        public bool hasDwm { get; set; }

        [JsonProperty(PropertyName = "has-intraday")]
        public bool hasIntraday { get; set; }

        [JsonProperty(PropertyName = "has-no-volume")]
        public bool[] hasNoVolume { get; set; }
        public string[] type { get; set; }
        public string[] ticker { get; set; }
        public string timezone { get; set; }

        [JsonProperty(PropertyName = "session-regular")]
        public string sessionRegular { get; set; }
               
       
       
    }

    public class HistoryAPI
    {
        public double[] c { get; set; }
        public double[] h { get; set; }
        public double[] l { get; set; }
        public double[] o { get; set; } 
        public string s { get; set; }
        public long[] t { get; set; }
        public long[] v { get; set; }
    }
   
}