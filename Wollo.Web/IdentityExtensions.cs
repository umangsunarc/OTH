using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Wollo.Web
{
    public static class IdentityExtensions
    {
        public static string GetOrganizationId(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("OrganizationId").Value;
        }
    }
}