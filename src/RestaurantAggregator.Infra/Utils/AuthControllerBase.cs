using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Infra.Utils;

public class AuthControllerBase : ControllerBase
{
    protected Guid UserId => Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
}
