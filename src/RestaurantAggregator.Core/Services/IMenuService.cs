using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IMenuService
{
    Task<ICollection<MenuDTO>> GetMenusByRestaurantIdAsync(Guid restaurantId);
    Task<MenuDetails> GetMenuByIdAsync(Guid id);
    Task CreateMenuAsync(MenuCreation menu);
    Task UpdateMenuAsync(MenuCreation menu, Guid id);
    Task DeleteMenuAsync(Guid id);
    Task DeleteDishFromMenuAsync(Guid menuId, Guid dishId);
    Task AddDishToMenuAsync(Guid menuId, Guid dishId);
}
