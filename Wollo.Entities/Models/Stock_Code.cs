using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Stock_Code : AuditableEntity
    {
        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string stock_name { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string stock_code { get; set; }
    }
}
