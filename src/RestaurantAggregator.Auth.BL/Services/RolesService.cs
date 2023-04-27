using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.BL.Services;

public class RolesService : IRolesService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public RolesService(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task AddRoleToUserAsync(Guid userId, RoleType roleType)
    {
        var role = await _roleManager.FindByNameAsync(roleType.ToString());
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || role is null)
        {
            throw new NotFoundInDbException($"User with id {userId} or role {roleType} not found");
        }
        await _userManager.AddToRoleAsync(user, role.Name);
    }

    public async Task<IEnumerable<RoleType>> GetUserRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundInDbException($"User with id {userId} not found");
        }
        var roles = await _userManager.GetRolesAsync(user);
        return roles.Select(Enum.Parse<RoleType>);
    }

    public async Task RemoveRoleFromUserAsync(Guid userId, RoleType roleType)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundInDbException($"User with id {userId} not found");
        }
        await _userManager.RemoveFromRoleAsync(user, roleType.ToString());
    }
}
