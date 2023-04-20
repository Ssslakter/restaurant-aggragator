using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IMenuService
{
    Task<ICollection<MenuDTO>> GetMenusByRestaurantIdAsync(Guid restaurantId);
    Task<MenuDetails> GetMenuByIdAsync(Guid id, ICollection<Category> filters,
        Sorting sorting, uint page);
    Task<MenuDTO> CreateMenuAsync(MenuCreation menu, Guid restaurantId);
    Task<MenuDTO> UpdateMenuAsync(MenuCreation menu, Guid id);
    Task DeleteMenuAsync(Guid id);
    Task RemoveDishFromMenuAsync(Guid menuId, Guid dishId);
    Task AddDishToMenuAsync(Guid menuId, Guid dishId);
}
