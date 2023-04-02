namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable

public class RestaurantDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<MenuDTO> Menus { get; set; }
}
