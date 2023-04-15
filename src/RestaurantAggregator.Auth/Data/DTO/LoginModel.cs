using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class LoginModel
{
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}
