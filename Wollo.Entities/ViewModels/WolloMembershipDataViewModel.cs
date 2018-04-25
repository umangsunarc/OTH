using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    public class WolloMembershipDataViewModel
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email_address { get; set; }
        public string username { get; set; }
        public bool is_subscribed { get; set; }
        public DateTime? dob { get; set; }
        public int gender { get; set; }
        public string telephone { get; set; }
        public string[] street { get; set; }
        public string city { get; set; }
        public int region_id { get; set; }
        public string region { get; set; }
        public string postcode { get; set; }
        public int country_id { get; set; }
        public string password { get; set; }
        public string confirmation { get; set; }
        public string company { get; set; }
    }
}
