using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class EmailDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
