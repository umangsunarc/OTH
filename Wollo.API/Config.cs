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
        public string value { get; set; }
        public string name { get; set; }        
    }
   
}