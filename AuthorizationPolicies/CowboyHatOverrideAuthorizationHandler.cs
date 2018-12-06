using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationPolicies
{
    public class CowboyHatOverrideAuthorizationHandler : AuthorizationHandler<LocationRequirement>
    {

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LocationRequirement requirement)
        {
            var wearingCowboyHat = Convert.ToBoolean(
                context.User.FindFirst(
                    c => c.Type == "contoso:CowboyHat" &&
                    c.Issuer == "urn:PassportControl").Value);

            if (wearingCowboyHat)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
