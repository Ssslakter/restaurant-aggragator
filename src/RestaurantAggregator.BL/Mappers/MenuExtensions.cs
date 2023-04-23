using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Mappers;

public static class MenuExtensions
{
    public static MenuDTO ToDTO(this Menu menu)
    {
        return new MenuDTO
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
            RestaurantId = menu.RestaurantId
        };
    }

    public static MenuDetails ToDetails(this Menu menu, IEnumerable<Dish> filteredDishes)
    {
        return new MenuDetails
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
            RestaurantId = menu.RestaurantId,
            Dishes = filteredDishes.Select(d => d.ToDTO()).ToList()
        };
    }
}
