using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Client.Services;
using RestaurantAggregator.BL.Mappers;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RestaurantAggregator.DAL.Data;
using ProfileDTO = RestaurantAggregator.Auth.Client.Services.ProfileDTO;
using RoleTypeCore = RestaurantAggregator.Core.Data.Enums.RoleType;

namespace RestaurantAggregator.BL.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly IUserService _userService;
    private readonly int _pageSize = 10;

    public RestaurantService(RestaurantDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<RestaurantDTO> CreateRestaurantAsync(RestaurantCreation restaurantModel)
    {
        var restaurantEntity = new Restaurant
        {
            Name = restaurantModel.Name,
            Menus = new List<Menu>(),
            Cooks = new List<Guid>(),
            Managers = new List<Guid>()
        };
        await _context.Restaurants.AddAsync(restaurantEntity);
        await _context.SaveChangesAsync();
        return new RestaurantDTO
        {
            Id = restaurantEntity.Id,
            Name = restaurantEntity.Name,
            Menus = restaurantEntity.Menus.Select(m => m.ToDTO()).ToList()
        };
    }

    public async Task DeleteRestaurantAsync(Guid id)
    {
        var result = await _context.Restaurants.Where(r => r.Id == id).ExecuteDeleteAsync();
        if (result == 0)
        {
            throw new NotFoundInDbException($"Restaurant with id {id} not found");
        }
    }

    public async Task<ICollection<RestaurantDTO>> GetRestaurantsAsync(uint page)
    {
        var restaurants = _context.Restaurants.Include(r => r.Menus).Select(r => new RestaurantDTO
        {
            Id = r.Id,
            Name = r.Name,
            Menus = r.Menus.Select(m => m.ToDTO()).ToList()
        }).OrderBy(r => r.Name).Skip((int.Max(1, (int)page) - 1) * _pageSize).Take(_pageSize);
        return await restaurants.ToListAsync();
    }

    public async Task<ICollection<RestaurantDTO>> GetRestaurantsByNameAsync(string name, uint page)
    {
        return await _context.Restaurants.Include(r => r.Menus).Where(r => r.Name.Contains(name)).Select(r => new RestaurantDTO
        {
            Id = r.Id,
            Name = r.Name,
            Menus = r.Menus.Select(m => m.ToDTO()).ToList()
        }).OrderBy(r => r.Name).Skip((int.Max(1, (int)page) - 1) * _pageSize).Take(_pageSize).ToListAsync();
    }

    public async Task<IEnumerable<ProfileWithRolesDTO>> GetRestaurantStaffAsync(Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
        if (restaurant == null)
        {
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        }
        var cookIds = restaurant.Cooks;
        var managerIds = restaurant.Managers;
        return await Task.WhenAll(cookIds.Concat(managerIds).Distinct().ToList()
            .ConvertAll(async id =>
            {
                var restaurantRoles = new List<RoleTypeCore>();
                if (cookIds.Contains(id))
                {
                    restaurantRoles.Add(RoleTypeCore.Cook);
                }
                if (managerIds.Contains(id))
                {
                    restaurantRoles.Add(RoleTypeCore.Manager);
                }
                return (await _userService.GetProfileAsync(id))
                .ToProfileWithRoles(restaurantRoles);
            }));
    }

    public async Task<RestaurantDTO> UpdateRestaurantAsync(Guid id, RestaurantCreation restaurantModel)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        if (restaurant == null)
        {
            throw new NotFoundInDbException($"Restaurant with id {id} not found");
        }
        restaurant.Name = restaurantModel.Name;
        await _context.SaveChangesAsync();
        return new RestaurantDTO
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Menus = restaurant.Menus.Select(m => m.ToDTO()).ToList()
        };
    }
}

internal static class ProfileDtoExtensions
{
    internal static ProfileWithRolesDTO ToProfileWithRoles(this ProfileDTO profile, IEnumerable<RoleTypeCore> roles)
    {
        return new ProfileWithRolesDTO
        {
            Id = profile.Id,
            Email = profile.Email,
            Name = profile.Name,
            Surname = profile.Surname,
            MiddleName = profile.MiddleName,
            BirthDate = profile.BirthDate is null ? null : DateOnly.FromDateTime(((DateTimeOffset)profile.BirthDate).Date),
            Gender = (Core.Data.Enums.Gender?)profile.Gender,
            Phone = profile.Phone,
            Roles = roles
        };
    }
}
