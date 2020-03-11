using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ToDo.Api.Extensions
{
    public static class UserExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault
                (c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email").Value;
        }
    }
}
