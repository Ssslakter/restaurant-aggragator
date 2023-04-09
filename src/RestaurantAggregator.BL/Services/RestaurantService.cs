using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;

namespace RestaurantAggregator.BL.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;

    public RestaurantService(RestaurantDbContext context)
    {
        _context = context;
    }
    public async Task<ICollection<RestaurantDTO>> GetRestaurantsAsync()
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
        });
        return await restaurants.ToListAsync();
    }
}
