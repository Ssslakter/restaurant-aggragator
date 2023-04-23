using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Infra.Auth;

public class AuthControllerBase : ControllerBase
{
    protected Guid UserId => Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    protected List<RoleType> UserRoles => User.Claims.Where(x => x.Type == ClaimTypes.Role)
    .Select(x => Enum.Parse<RoleType>(x.Value)).ToList();
}
