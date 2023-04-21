using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Auth.Data.DTO;
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
}
