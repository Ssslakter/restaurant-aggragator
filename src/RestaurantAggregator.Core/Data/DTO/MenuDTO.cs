namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable

public class MenuDTO : MenuCreation
{
    public Guid Id { get; set; }
}

public class MenuCreation
{
    public string Name { get; set; }
    public string Description { get; set; }
}