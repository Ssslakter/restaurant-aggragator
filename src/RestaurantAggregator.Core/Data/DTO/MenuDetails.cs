namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable

public class MenuDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<DishDTO> Dishes { get; set; }
}
