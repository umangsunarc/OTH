using Wollo.Base.Entity;
namespace Wollo.Entities.Models
{
    public class RPE_Portal_User : AuditableEntity
    {
        public string user_name { get; set; }
        public string email { get; set; }
    }
}
