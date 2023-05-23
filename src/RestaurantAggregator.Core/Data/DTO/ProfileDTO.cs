using System.ComponentModel.DataAnnotations;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data.DTO;

public class ProfileDTO
{
    public Guid Id { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    [Phone]
    public string? Phone { get; set; }
}