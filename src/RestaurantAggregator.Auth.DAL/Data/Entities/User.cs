using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Auth.DAL.Data.Entities;

public class User : IdentityUser<Guid>
{
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public Courier? Courier { get; set; }
    public Client? Client { get; set; }
    public Manager? Manager { get; set; }
    public Cook? Cook { get; set; }
    public bool IsBanned { get; set; }
}
