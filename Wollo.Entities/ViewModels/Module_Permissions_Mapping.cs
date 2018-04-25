using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    class Module_Permissions_Mapping : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [Required(ErrorMessage = "module id is required.")]
        [ForeignKey("Module")]
        public int moduleid { get; set; }
        [DataMember]
        [Required(ErrorMessage = "permission id is required.")]
        [ForeignKey("PermissionsMaster")]
        public int permissionid { get; set; }

        [DataMember]
        public virtual Module Module { get; set; }
        [DataMember]
        public virtual Permissions_Master PermissionsMaster { get; set; }
    }
}
