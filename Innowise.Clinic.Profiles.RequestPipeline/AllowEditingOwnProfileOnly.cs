using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innowise.Clinic.Profiles.RequestPipeline;

public class AllowInteractionWithOwnProfileOnlyFilter : ActionFilterAttribute
{
    private readonly List<string> _rolesWithLimitedPermissions;

    public AllowInteractionWithOwnProfileOnlyFilter(string userRoles)
    {
        _rolesWithLimitedPermissions = userRoles.Split(",").Select(x => x.Trim()).ToList();
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_rolesWithLimitedPermissions.Any(x => context.HttpContext.User.IsInRole(x)))
        {
            var profileIdClaim =
                context.HttpContext.User.Claims.FirstOrDefault(x =>
                    x.Type == JwtClaimTypes.LimitedAccessToProfileClaim);

            if (!(context.ActionArguments["id"] is Guid requestedId && profileIdClaim != null &&
                  requestedId.ToString() == profileIdClaim.Value))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        await next();
    }
}