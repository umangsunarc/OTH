using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class AspnetUserroles
    {
        [DataMember]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        [DataMember]
        [ForeignKey("AspNetRoles")]
        public string RoleId { get; set; }
        public virtual AspnetRoles AspNetRoles { get; set; }
        public virtual AspnetUsers AspNetUsers { get; set; }
    }
}
