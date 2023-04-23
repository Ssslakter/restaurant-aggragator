using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Mappers;

public static class DishExtensions
{
    public static DishDTO ToDTO(this Dish dish)
    {
        return new DishDTO
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            IsVegeterian = dish.IsVegeterian,
            Photo = dish.Photo,
            Category = dish.Category,
            MenuId = dish.MenuId,
            RestaurantId = dish.RestaurantId,
            Rating = dish.Reviews.Any() ? dish.Reviews.Average(r => r.Value) : 0
        };
    }
}
