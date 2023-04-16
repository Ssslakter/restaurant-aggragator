using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class DeliveryAddress
{
    [Required]
    public string City { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string House { get; set; }
}
