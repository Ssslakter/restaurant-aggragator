namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Menu> Menus { get; set; }
    public List<Guid> Cooks { get; set; }
    public List<Guid> Managers { get; set; }
}
