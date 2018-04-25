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
    class Module : Entity
    {
        [DataMember]
        [MaxLength(45, ErrorMessage = "name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "name cannot be smaller than 3 characters.")]
        public string name { get; set; }
    }
}
