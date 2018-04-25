using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Holiday_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "description is required.")]
        //[MaxLength(45, ErrorMessage = "description cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "description cannot be smaller than 3 characters.")]
        public string description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "holiday date is required.")]

        public DateTime holiday_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "holiday statusid is required.")]
        [ForeignKey("HolidayStatusMaster")]
        public int holiday_statusid { get; set; }

        [DataMember]
        public int notify_before { get; set; }

        [DataMember]
        public virtual Holiday_Status_Master HolidayStatusMaster { get; set; }
    }
}
