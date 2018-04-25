using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.Models  
{
    [DataContract]
    public class Market_Rate_Details : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "Rate is required.")]
        public float rate { get; set; }
    }
}
