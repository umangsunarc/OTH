using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace Wollo.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public bool IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public ApplicationUser()
        {
            this.OrganizationDetails = new HashSet<organizationdetails>();
        }

        public virtual ICollection<organizationdetails> OrganizationDetails { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {

    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MySQLConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<permission> Permission { get; set; }
        public DbSet<module> Module { get; set; }
        public DbSet<module_permissions_mapping> ModulePermissionMapping { get; set; }
        public DbSet<rolemodulepermissionMapping> RoleModulePermissionMapping { get; set; }
        public DbSet<organization> Organization { get; set; }
        public DbSet<organizationdetails> OrganizationDetails { get; set; }
        public DbSet<Wollo.Entities.Models.User> AppUsers { get; set; }
    }
}