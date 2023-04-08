using RestaurantAggregator.Auth.Data.Enums;

namespace RestaurantAggregator.Auth.Data.Entities;
#nullable disable
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Role> Roles { get; set; }
}
