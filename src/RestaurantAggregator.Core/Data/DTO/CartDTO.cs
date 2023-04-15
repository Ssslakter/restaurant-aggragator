namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class CartDTO
{
    public Guid ClientId { get; set; }
    public ICollection<DishInCartDTO> Dishes { get; set; }
}