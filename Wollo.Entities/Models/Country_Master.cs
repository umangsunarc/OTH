using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    [DataContract]
    //[TrackChanges]
    public class Country_Master : Entity
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string code { get; set; }

    }
}
