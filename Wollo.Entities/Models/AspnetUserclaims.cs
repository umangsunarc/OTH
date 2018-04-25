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
    class AspnetUserclaims : Entity
    {
        [DataMember]
        [MaxLength(400, ErrorMessage = "claim type cannot be longer than 400 characters.")]
        [MinLength(3, ErrorMessage = "claim type cannot be smaller than 3 characters.")]
        public string claimtype { get; set; }
        [DataMember]
        [MaxLength(400, ErrorMessage = "claim value cannot be longer than 400 characters.")]
        [MinLength(3, ErrorMessage = "claim value cannot be smaller than 3 characters.")]
        public string claimvalue { get; set; }
        [DataMember]
        [ForeignKey("AspNetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public virtual AspnetUsers AspNetUsers { get; set; }
    }
}
