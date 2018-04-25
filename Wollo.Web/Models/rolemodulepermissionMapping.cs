using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    [Table("RoleModulePermissionMapping")]
    public class rolemodulepermissionMapping
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }

        public int ModulePermissionMappingId { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public virtual module_permissions_mapping ModulePermissionMapping { get; set; }
    }
}