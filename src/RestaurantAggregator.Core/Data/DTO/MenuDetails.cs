namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable

public class MenuDetails : MenuDTO
{
    public ICollection<DishDTO> Dishes { get; set; }
}
