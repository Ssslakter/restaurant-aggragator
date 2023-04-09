using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Core.Data.DTO;

public class ReviewDTO
{
    [Range(1, 10)]
    public uint Value { get; set; }
}
