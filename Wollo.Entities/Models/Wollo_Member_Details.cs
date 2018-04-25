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
    public class Wollo_Member_Details : AuditableEntity
    {
        [DataMember]
        //[MaxLength(45, ErrorMessage = "member id cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "member id cannot be smaller than 3 characters.")]
        public string user_name { get; set; }

        [DataMember]
        //[MaxLength(45, ErrorMessage = "first name cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "first name cannot be smaller than 3 characters.")]
        public string first_name { get; set; }

        [DataMember]
        //[MaxLength(45, ErrorMessage = "last name cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "last name cannot be smaller than 3 characters.")]
        public string last_name { get; set; }

        [DataMember]
        //[MaxLength(45, ErrorMessage = "email address cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "email address cannot be smaller than 3 characters.")]
        public string email_address { get; set; }

        [DataMember]
        //[MaxLength(45, ErrorMessage = "address cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "address cannot be smaller than 3 characters.")]
        public bool is_subscribed { get; set; }

        [DataMember]
        //[MaxLength(45, ErrorMessage = "alternate address cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "alternate address cannot be smaller than 3 characters.")]
        public DateTime? dob { get; set; }
        [DataMember]
        //[MaxLength(45, ErrorMessage = "phone numbe cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "phone numbe cannot be smaller than 3 characters.")]
        public int gender { get; set; }
        [DataMember]
        //[MaxLength(45, ErrorMessage = "wollo member detailscol cannot be longer than 45 characters.")]
        //[MinLength(3, ErrorMessage = "wollo member detailscol cannot be smaller than 3 characters.")]
        public string telephone { get; set; }
        [DataMember]
        public string street { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public int region_id { get; set; }
        [DataMember]
        public string region { get; set; }
        [DataMember]
        public string postcode { get; set; }
        [DataMember]
        public int country_id { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string confirmation { get; set; }
        [DataMember]
        public string company { get; set; }
    }
}
