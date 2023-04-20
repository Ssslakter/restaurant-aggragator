using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class DishDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual decimal Price { get; set; }
    public bool IsVegeterian { get; set; }
    public string Photo { get; set; }
    public Category Category { get; set; }
    public Guid MenuId { get; set; }
    public Guid RestaurantId { get; set; }
    public double Rating { get; set; }
}

public class DishInCartDTO : DishDTO
{
    public uint Count { get; set; }
}

public class DishInOrderDTO : DishDTO
{
    public uint Count { get; set; }
    public override decimal Price { get; set; }
}
