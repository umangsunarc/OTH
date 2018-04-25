using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class AspnetUsers
    {
        [DataMember]
        //[ForeignKey("AspnetUserroles")]
        public string Id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "First Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "First Name cannot be smaller than 3 characters.")]
        public string firstname { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "Last Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Last Name cannot be smaller than 3 characters.")]
        public string lastname { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "email cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "email cannot be smaller than 3 characters.")]
        public string email { get; set; }
        [DataMember]
        public bool? emailconfirmed { get; set; }

        [DataMember]
        [Required(ErrorMessage = "password hash action is required.")]
        public string passwordhash { get; set; }

        [DataMember]
        [Required(ErrorMessage = "security stamp is required.")]
        public string securitystamp { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "phone number cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "phone number cannot be smaller than 3 characters.")]
        public string phonenumber { get; set; }

        [DataMember]
        public bool? phonenumberconfirmed { get; set; }

        [DataMember]
        public bool? twofactorenabled { get; set; }

        [DataMember]
        public DateTime? lockoutenddateutc { get; set; }

        [DataMember]
        public bool? lockoutenabled { get; set; }

        [DataMember]
        public int? accessfailedcount { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "username cannot be smaller than 3 characters.")]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }
        [DataMember]
        [MaxLength(128, ErrorMessage = "discriminator cannot be longer than 128 characters.")]
        [MinLength(3, ErrorMessage = "discriminator cannot be smaller than 3 characters.")]
        public string discriminator { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        //[DataMember]
        //public virtual AspnetUserroles AspnetUserroles { get; set; }

    }
}
