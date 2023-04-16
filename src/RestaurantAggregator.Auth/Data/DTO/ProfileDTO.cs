using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class ProfileDTO
{
    [EmailAddress]
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    public DateOnly BirthDate { get; set; }
    public Core.Data.Enums.Gender Gender { get; set; }
    [Phone]
    public string Phone { get; set; }
}