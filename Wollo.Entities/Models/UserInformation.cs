using Wollo.Base.Entity;
using System;

namespace Wollo.Entities.Models
{
    public class UserInformation : AuditableEntity
    {
        public string PortalUserId { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime SessionStartDateTime { get; set; }
        public string UserName { get; set; }
    }
}
