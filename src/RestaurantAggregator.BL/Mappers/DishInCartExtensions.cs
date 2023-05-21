using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Mappers;

public static class DishInCartExtensions
{
    public static DishInCartDTO ToDTO(this DishInCart dishInCart)
    {
        return new DishInCartDTO
        {
            Id = dishInCart.Id,
            Name = dishInCart.Dish.Name,
            Description = dishInCart.Dish.Description,
            Price = dishInCart.Price,
            IsVegeterian = dishInCart.Dish.IsVegeterian,
            Photo = dishInCart.Dish.Photo,
            Category = dishInCart.Dish.Category,
            RestaurantId = dishInCart.Dish.RestaurantId,
            Count = dishInCart.Count
        };
    }

    public static DishInOrderDTO ToOrderDTO(this DishInCart dishInCart, decimal price)
    {
        return new DishInOrderDTO
        {
            Id = dishInCart.Id,
            Name = dishInCart.Dish.Name,
            Description = dishInCart.Dish.Description,
            Price = price,
            IsVegeterian = dishInCart.Dish.IsVegeterian,
            Photo = dishInCart.Dish.Photo,
            Category = dishInCart.Dish.Category,
            RestaurantId = dishInCart.Dish.RestaurantId,
            Count = dishInCart.Count
        };
    }
}
