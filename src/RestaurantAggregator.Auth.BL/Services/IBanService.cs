namespace RestaurantAggregator.Auth.BL.Services;

public interface IBanService
{
    Task BanUserAsync(Guid userId);
    Task UnbanUserAsync(Guid userId);
    Task<bool> IsBannedAsync(Guid userId);
}