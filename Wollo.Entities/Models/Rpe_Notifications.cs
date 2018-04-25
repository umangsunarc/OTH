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
    class Rpe_Notifications : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "description is required.")]
        [MaxLength(200, ErrorMessage = "description cannot be longer than 200 characters.")]
        [MinLength(3, ErrorMessage = "description cannot be smaller than 3 characters.")]
        public string description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "notification statusid is required.")]
        [ForeignKey("NotificationStatusMaster")]
        public int notification_statusid { get; set; }
        [DataMember]
        public virtual Notification_Status_Master NotificationStatusMaster { get; set; }
    }
}
