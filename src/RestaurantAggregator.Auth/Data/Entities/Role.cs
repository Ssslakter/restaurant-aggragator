using RestaurantAggregator.Auth.Data.Enums;

namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class Role
{
    public int Id { get; set; }
    public RoleType RoleType { get; set; }
    public ICollection<UserRole> Users { get; set; }
}
