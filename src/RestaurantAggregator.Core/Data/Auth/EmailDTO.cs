using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.Auth;
#nullable disable
public class EmailDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
