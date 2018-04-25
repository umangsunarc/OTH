using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    public class ModulePermissionMappingViewModel
    {
        [Key]
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public virtual module Module { get; set; }
        public virtual List<PermissionViewModel> Permissions { get; set; }
        public virtual List<rolemodulepermissionMapping> RoleModulePermissionMapping { get; set; }
    }
}