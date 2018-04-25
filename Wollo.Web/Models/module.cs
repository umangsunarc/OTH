﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    [Table("Module")]
    public class module
    {
        public module()
        {
            this.ModulePermissionMapping = new HashSet<module_permissions_mapping>();
        }

        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Module Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string name { get; set; }

        public virtual ICollection<module_permissions_mapping> ModulePermissionMapping { get; set; }
    }
}