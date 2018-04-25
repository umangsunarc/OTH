using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    [Table("ModulePermissionMapping")]
    public class module_permissions_mapping
    {
        public module_permissions_mapping()
        {
            this.RoleModulePermissionMapping = new HashSet<rolemodulepermissionMapping>();
        }

        [Key]
        public int id { get; set; }

        public int moduleid { get; set; }

        public int permissionid { get; set; }

        public virtual module Module { get; set; }
        public virtual permission Permission { get; set; }
        public virtual ICollection<rolemodulepermissionMapping> RoleModulePermissionMapping { get; set; }
    }
}