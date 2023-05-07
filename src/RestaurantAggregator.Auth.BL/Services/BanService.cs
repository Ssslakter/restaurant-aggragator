using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.BL.Services;

public class BanService : IBanService
{
    private readonly UserManager<User> _userManager;

    public BanService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task BanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        user.IsBanned = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> IsBannedAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        return user.IsBanned;
    }

    public async Task UnbanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        user.IsBanned = false;
        await _userManager.UpdateAsync(user);
    }
}