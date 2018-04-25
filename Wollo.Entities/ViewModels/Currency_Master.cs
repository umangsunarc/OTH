using Wollo.Base.Entity;
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
    class Currency_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "name is required.")]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string name { get; set; }

        [DataMember]
        public int is_default { get; set; }
    }
}
