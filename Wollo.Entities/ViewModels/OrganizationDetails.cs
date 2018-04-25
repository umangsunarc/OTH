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
    class OrganizationDetails
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]

        public int organizationid { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "userid cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "userid cannot be smaller than 3 characters.")]
        public string userid { get; set; }

        [DataMember]

        public bool isadmin { get; set; }
    }
}
