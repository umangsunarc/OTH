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
    class Issue_Points_Details : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "account id is required.")]
        public int account_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points is required.")]

        public int points { get; set; }
    }
}
