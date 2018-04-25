using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Cash_Transaction_History : AuditableEntity
    {
        [DataMember]
        [ForeignKey("AspNetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public float opening_cash { get; set; }
        [DataMember]
        public float transaction_amount { get; set; }
        [DataMember]
        public float closing_cash { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Description cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Description cannot be smaller than 3 characters.")]
        public string description { get; set; }
        [DataMember]
        public virtual AspnetUsers AspNetUsers { get; set; }
    }
}
