using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wollo.Web.Models
{
    [Table("OrganizationDetails")]
    public class organizationdetails
    {
        [Key]
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public string UserId { get; set; }

        public bool IsAdmin { get; set; }

        public virtual organization Organization { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}