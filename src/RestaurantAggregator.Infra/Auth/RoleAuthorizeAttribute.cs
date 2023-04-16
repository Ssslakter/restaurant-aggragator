using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Infra.Auth;

public class RoleAuthorizeAttribute : TypeFilterAttribute
{
    public RoleAuthorizeAttribute(params RoleType[] roles) : base(typeof(RoleAuthorizeFilter))
    {
        Arguments = new object[] { roles };
    }
}

public class RoleAuthorizeFilter : IAuthorizationFilter
{
    private readonly RoleType[] _roles;

    public RoleAuthorizeFilter(params RoleType[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
#nullable disable
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
#nullable enable

        if (!HasRequiredRole(user))
        {
            context.Result = new ForbidResult();
        }
    }

    private bool HasRequiredRole(ClaimsPrincipal user)
    {
        foreach (var roleClaim in user.FindAll(c => c.Type == ClaimTypes.Role))
        {
            var role = Enum.Parse<RoleType>(roleClaim.Value);
            if (_roles.Contains(role))
                return true;
        }

        return false;
    }
}