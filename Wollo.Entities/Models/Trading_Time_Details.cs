using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Trading_Time_Details : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "start time is required.")]
        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan? start_time { get; set; }

        [DataMember]
        [Required(ErrorMessage = "end time is required.")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan? end_time { get; set; }

        //[DataMember]
        //[Required(ErrorMessage = "start day is required.")]
        //[MaxLength(45, ErrorMessage = "start day cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "start daycannot be smaller than 3 characters.")]
        //public string start_day { get; set; }

        //[DataMember]
        //[Required(ErrorMessage = "end day is required.")]
        //[MaxLength(45, ErrorMessage = "end day cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "end day cannot be smaller than 3 characters.")]
        //public string end_day { get; set; }
    }
}
