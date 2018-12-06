using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationPolicies
{
    public class LocationAuthorizationHandler : AuthorizationHandler<LocationRequirement>
    {
        IWeatherProvider _weatherProvider;

        public LocationAuthorizationHandler(IWeatherProvider weatherProvider)
        {
            _weatherProvider = weatherProvider;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LocationRequirement requirement)
        {
            var homeTown =
                context.User.FindFirst(
                    c => c.Type == "contoso:HomeTown" &&
                    c.Issuer == "urn:PassportControl").Value.ToUpperInvariant();

            switch (requirement.Location)
            {
                case Location.Outside:

                    if (homeTown == "SEATTLE")
                    {
                        context.Succeed(requirement);
                    }
                    else if (_weatherProvider.GetSeason() != Season.Winter &&
                        homeTown == "LASVEGAS")
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        // Do nothing, this is not when these people should be outside.
                        ;
                    }
                    break;

                case Location.Inside:
                    context.Succeed(requirement);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
