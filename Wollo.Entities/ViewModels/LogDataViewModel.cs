using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    public class LogDataViewModel
    {
        //***************************************** Label properties start ***************************************************//
        [DataMember]
        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string log_details { get; set; }
        [DataMember]
        [Display(Name = "URL", ResourceType = typeof(Resource))]
        public string url { get; set; }
        [DataMember]
        [Display(Name = "IpAddress", ResourceType = typeof(Resource))]
        public string ip_address { get; set; }
        [DataMember]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }
        [DataMember]
        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public string date_time { get; set; }
        //***************************************** Label properties end *****************************************************//
        public List<Audit_Log_Master> AuditLogMaster { get; set; }
    }
}
