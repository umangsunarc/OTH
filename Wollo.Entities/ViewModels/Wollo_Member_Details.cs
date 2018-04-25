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
    class Wollo_Member_Details
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "member id cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "member id cannot be smaller than 3 characters.")]
        public string member_id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "first name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "first name cannot be smaller than 3 characters.")]
        public string first_name { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "last name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "last name cannot be smaller than 3 characters.")]
        public string last_name { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "email address cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "email address cannot be smaller than 3 characters.")]
        public string email_address { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "address cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "address cannot be smaller than 3 characters.")]
        public string address { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "alternate address cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "alternate address cannot be smaller than 3 characters.")]
        public string alternate_address { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "phone numbe cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "phone numbe cannot be smaller than 3 characters.")]
        public string phone_numbe { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "wollo member detailscol cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "wollo member detailscol cannot be smaller than 3 characters.")]
        public string wollo_member_detailscol { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "country code cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "country code cannot be smaller than 3 characters.")]
        public string country_code { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "country id cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "country id cannot be smaller than 3 characters.")]
        public string country_id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "city cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "city cannot be smaller than 3 characters.")]
        public string city { get; set; }
        [DataMember]

        public int zip { get; set; }
    }
}
