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
    public class AspnetRoles
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string Name { get; set; }
        [DataMember]
        [MaxLength(128, ErrorMessage = "Discriminator cannot be longer than 128 characters.")]
        [MinLength(3, ErrorMessage = "Discriminator cannot be smaller than 3 characters.")]
        public string Discriminator { get; set; }

    }
}
