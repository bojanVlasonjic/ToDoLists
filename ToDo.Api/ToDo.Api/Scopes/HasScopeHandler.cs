using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace ToDo.Api.Scopes
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {

            // if the user does not have a scope claim, get him out
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }

            // extract all of the user permissions
            var permissions = context.User.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer);

            //success if any of the user's permissions contain the controller required scope
            if(permissions.Any(s => s.Value == requirement.Scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
