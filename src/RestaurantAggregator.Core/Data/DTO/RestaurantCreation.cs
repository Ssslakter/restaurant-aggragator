using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class RestaurantCreation
{
    [Required]
    public string Name { get; set; }
}
