using System.Runtime.Serialization;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class country_master
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string code { get; set; }
    }
}
