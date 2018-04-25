using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel.DataAnnotations;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Audit_Log_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [Display(Name = "URL", ResourceType = typeof(Resource))]
        public string url { get; set; }
        [DataMember]
        [Display(Name = "IpAddress", ResourceType = typeof(Resource))]
        public string ip_address { get; set; }
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        //***************************** Label Properties start **********************************************//
        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string log_details { get; set; }

        [Display(Name = "DashboardLogDetails", ResourceType = typeof(Resource))]
        public string dashboard_log_details { get; set; }

        [Display(Name = "Refresh", ResourceType = typeof(Resource))]
        public string refresh { get; set; }
      
        //***************************** Label Properties start **********************************************//


    }
}
