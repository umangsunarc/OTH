using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    //[TrackChanges]
    public class organization : Entity
    {
        [DataMember]
        public string name { get; set; }

    }
}
