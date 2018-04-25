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
    class Company_Code : Entity
    {

        [DataMember]
        [MaxLength(45, ErrorMessage = "company name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "company name cannot be smaller than 3 characters.")]
        public string company_name { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "company code cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "company code cannot be smaller than 3 characters.")]
        public string company_code { get; set; }
    }
}
