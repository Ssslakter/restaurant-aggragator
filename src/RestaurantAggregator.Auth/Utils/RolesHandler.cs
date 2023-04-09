using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Data;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Auth.Data.Enums;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Utils;

public interface IRolesHandler
{
    public Task AddRoleToUserAsync(User user, Role role);
    public Task RemoveRoleFromUserAsync(User user, Role role);
}

public class RolesHandler : IRolesHandler
{
    private readonly AuthDbContext _context;

    public RolesHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task AddRoleToUserAsync(User user, Role role)
    {
        var userRole = await _context.UserRoles.SingleOrDefaultAsync(x => x.Role == role);
        if (userRole == null)
        {
            throw new NotFoundInDbException($"Role {role} not found");
        }
        if (user.Roles.Contains(userRole))
        {
            return;
        }
        user.Roles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoleFromUserAsync(User user, Role role)
    {
        var userRole = await _context.UserRoles.SingleOrDefaultAsync(x => x.Role == role);
        if (userRole == null)
        {
            throw new NotFoundInDbException($"Role {role} not found");
        }
        if (!user.Roles.Contains(userRole))
        {
            return;
        }
        user.Roles.Remove(userRole);
        await _context.SaveChangesAsync();
    }
}
