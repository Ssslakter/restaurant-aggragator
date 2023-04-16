using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;

namespace RestaurantAggregator.BL.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly int _pageSize = 10;

    public RestaurantService(RestaurantDbContext context)
    {
        _context = context;
    }
    public async Task<ICollection<RestaurantDTO>> GetRestaurantsAsync(uint page)
    {
        var restaurants = _context.Restaurants.Include(r => r.Menus).Select(r => new RestaurantDTO
        {
            Id = r.Id,
            Name = r.Name,
            Menus = r.Menus.Select(m => new MenuDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                RestaurantId = m.RestaurantId,
            }).ToList()
        }).OrderBy(r => r.Name).Skip(((int)page - 1) * _pageSize).Take(_pageSize);
        return await restaurants.ToListAsync();
    }

    public async Task<ICollection<RestaurantDTO>> GetRestaurantsByNameAsync(string name, uint page)
    {
        return await _context.Restaurants.Include(r => r.Menus).Where(r => r.Name.Contains(name)).Select(r => new RestaurantDTO
        {
            Id = r.Id,
            Name = r.Name,
            Menus = r.Menus.Select(m => new MenuDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                RestaurantId = m.RestaurantId,
            }).ToList()
        }).OrderBy(r => r.Name).Skip(((int)page - 1) * _pageSize).Take(_pageSize).ToListAsync();
    }
}
