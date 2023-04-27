using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.Auth;
#nullable disable
public class RegistrationModel
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }
}
