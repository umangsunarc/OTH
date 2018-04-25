using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    class Organization
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string name { get; set; }
        [DataMember]
        [MaxLength(250, ErrorMessage = "description cannot be longer than 250 characters.")]
        [MinLength(3, ErrorMessage = "description cannot be smaller than 3 characters.")]
        public string description { get; set; }
    }
}
