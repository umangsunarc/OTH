using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class JobInfo : AuditableEntity
    {
        [DataMember]
        [MaxLength(50)]
        public string JobName { get; set; }

        [DataMember]
        [MaxLength(50)]
        public string JobCode { get; set; }

        [DataMember]
        [MaxLength(50)]
        public string Description { get; set; }
        [DataMember]
        public bool? IsRunning { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool? IsModified { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }

        [DataMember]
        public virtual ScheduleType ScheduleType { get; set; }
    }
}
