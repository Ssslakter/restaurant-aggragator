using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.BL.Mappers;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Services;

public class MenuService : IMenuService
{
    private readonly RestaurantDbContext _context;
    private readonly int _pageSize = 10;
    public MenuService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task AddDishToMenuAsync(Guid menuId, Guid dishId)
    {
        var menu = await _context.Menus.FindAsync(menuId);
        if (menu == null)
            throw new NotFoundInDbException($"Menu with id {menuId} not found");
        var dish = await _context.Dishes.FindAsync(dishId);
        if (dish == null)
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        if (menu.Dishes.Contains(dish))
            throw new DbViolationException($"Dish with id {dishId} already exists in menu with id {menuId}");

        menu.Dishes.Add(dish);
        await _context.SaveChangesAsync();
    }

    public async Task<MenuDTO> CreateMenuAsync(MenuCreation menu, Guid restaurantId)
    {
        var menuEntity = new Menu
        {
            Name = menu.Name,
            Description = menu.Description,
            RestaurantId = restaurantId,
            Dishes = new List<Dish>()
        };
        await _context.Menus.AddAsync(menuEntity);
        await _context.SaveChangesAsync();
        return menuEntity.ToDTO();
    }

    public async Task RemoveDishFromMenuAsync(Guid menuId, Guid dishId)
    {
        var menu = await _context.Menus.FindAsync(menuId);
        if (menu == null)
            throw new NotFoundInDbException($"Menu with id {menuId} not found");
        var dish = await _context.Dishes.FindAsync(dishId);
        if (dish == null)
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        var result = menu.Dishes.Remove(dish);
        if (!result)
            throw new DbViolationException($"Dish with id {dishId} not found in menu with id {menuId}");
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMenuAsync(Guid id)
    {
        var result = await _context.Menus.Where(m => m.Id == id).ExecuteDeleteAsync();
        if (result == 0)
            throw new NotFoundInDbException($"Menu with id {id} not found");
        await _context.SaveChangesAsync();
    }

    public async Task<MenuDetails> GetMenuByIdAsync(Guid id, ICollection<Category>? filters, Sorting sorting, uint page)
    {
        var menu = await _context.Menus
            .Include(m => m.Dishes)
            .ThenInclude(d => d.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (menu == null)
            throw new NotFoundInDbException($"Menu with id {id} not found");
        var filteredDishes = menu.Dishes
            .FilterDishes(filters)
            .SortDishes(sorting)
            .Skip(((int)page - 1) * _pageSize)
            .Take(_pageSize);

        return menu.ToDetails(filteredDishes);
    }

    public async Task<ICollection<MenuDTO>> GetMenusByRestaurantIdAsync(Guid restaurantId)
    {
        var menus = await _context.Menus
            .Where(m => m.RestaurantId == restaurantId)
            .Select(m => m.ToDTO()).ToListAsync();
        if (!menus.Any())
            throw new NotFoundInDbException($"Menus for restaurant with id {restaurantId} not found");

        return menus;
    }

    public async Task<MenuDTO> UpdateMenuAsync(MenuCreation menu, Guid id)
    {
        var menuEntity = await _context.Menus.FindAsync(id);
        if (menuEntity == null)
            throw new NotFoundInDbException($"Menu with id {id} not found");
        menuEntity.Name = menu.Name;
        menuEntity.Description = menu.Description;
        await _context.SaveChangesAsync();
        return menuEntity.ToDTO();
    }
}

internal static class QueryableExtensions
{
    internal static IEnumerable<Dish> SortDishes(this IEnumerable<Dish> dishes, Sorting sorting)
    {
        return sorting switch
        {
            Sorting.AlphabetAsc => dishes.OrderBy(d => d.Name),
            Sorting.PriceAsc => dishes.OrderBy(d => d.Price),
            Sorting.ScoreAsc => dishes.OrderBy(d => d.Reviews.Any() ? d.Reviews.Average(r => r.Value) : 0),
            Sorting.AlphabetDesc => dishes.OrderByDescending(d => d.Name),
            Sorting.PriceDesc => dishes.OrderByDescending(d => d.Price),
            Sorting.ScoreDesc => dishes.OrderByDescending(d => d.Reviews.Any() ? d.Reviews.Average(r => r.Value) : 0),
            _ => dishes.OrderBy(d => d.Name)
        };
    }

    internal static IEnumerable<Dish> FilterDishes(this IEnumerable<Dish> dishes, ICollection<Category>? filters)
    {
        if (filters == null || filters.Count == 0)
            return dishes;
        return dishes.Where(d => filters.Contains(d.Category));
    }
}
