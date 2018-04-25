using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    [Table("Organization")]
    public class organization
    {
        public organization()
        {
            this.OrganizationDetails = new HashSet<organizationdetails>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Organization")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Description { get; set; }

        public virtual ICollection<organizationdetails> OrganizationDetails { get; set; }
    }
}