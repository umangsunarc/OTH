using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    public class ModuleViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<PermissionViewModel> Permissions { get; set; }
    }
}