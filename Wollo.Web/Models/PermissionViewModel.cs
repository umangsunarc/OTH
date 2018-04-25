using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    public class PermissionViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public virtual List<ModuleViewModel> Modules { get; set; }
    }
}