using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class AuditLog
    {

        #region Constructor

        public AuditLog()
        {
            //AuditLogDetails = new List<TrackerEnabledDbContext.Common.Models.AuditLogDetail>();
        }

        #endregion
        [DataMember]
        public long auditlog_id { get; set; }

        [DataMember]
        public string user_id { get; set; }

        //[DataMember]
        //public string strUserName { get; set; }

        [DataMember]
        public DateTime event_date { get; set; }

        //[DataMember]
        //public TrackerEnabledDbContext.Common.Models.EventType EventType { get; set; }

        [DataMember]
        public int event_type { get; set; }

        [DataMember]
        public string type_fullname { get; set; }

        [DataMember]
        public int record_id { get; set; }

        //[DataMember]
        //public List<TrackerEnabledDbContext.Common.Models.AuditLogDetail> AuditLogDetails { get; set; }
    }
}
