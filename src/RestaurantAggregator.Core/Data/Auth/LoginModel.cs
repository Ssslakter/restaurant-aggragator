using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.Auth;
#nullable disable
public class LoginModel
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}
