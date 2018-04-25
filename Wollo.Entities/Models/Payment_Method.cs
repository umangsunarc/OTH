using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.Models
{
    public class Payment_Method : AuditableEntity
    {
        [DataMember]
        public string method { get; set; }
    }
}
