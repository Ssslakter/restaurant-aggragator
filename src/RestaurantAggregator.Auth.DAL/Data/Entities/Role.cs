using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Auth.DAL.Data.Entities;
#nullable disable
public class Role : IdentityRole<Guid>
{
    public override string Name { get; set; }
    public RoleType RoleType { get; set; }
}
