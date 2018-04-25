using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Audit_Log_Master : AuditableEntity
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string ip_address { get; set; }
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
    }
}
