using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wollo.Web.Models
{
    public class IdentityConfig
    {
        // This is useful if you do not want to tear down the database each time you run the application.
        // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        // This example shows you how to create a new database if the Model changes
        public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                InitializeIdentityForEF(context);
                base.Seed(context);
            }

            //Create User=admin with password=123456 in the Admin role        
            public static void InitializeIdentityForEF(ApplicationDbContext db)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
                const string name = "admin";
                const string password = "123456";
                const string roleName = "Admin";

                //Create Role Admin if it does not exist
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    var roleresult = roleManager.Create(role);
                }

                var user = userManager.FindByName(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name };
                    var result = userManager.Create(user, password);
                }
                userManager.AddToRole(user.Id, roleName);
            }
        }
    }
}