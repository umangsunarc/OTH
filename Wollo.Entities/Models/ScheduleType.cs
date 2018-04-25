using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class ScheduleType : AuditableEntity
    {
        [DataMember]
        public string CronExpression { get; set; }
        [DataMember]
        public string ScheduleName { get; set; }

        [DataMember]
        public virtual ICollection<JobInfo> JobInfos { get; set; }
    }
}
