using RestaurantAggregator.Core.Exceptions;
using StackExchange.Redis;

namespace RestaurantAggregator.Auth.Utils;

public interface ITokenStorage
{
    Task AddTokenAsync(string token, string userId);
    Task<bool> IsTokenActiveAsync(string token);
}

public class TokenStorage : ITokenStorage
{
    private readonly IDatabase _redisDatabase;
    private readonly TimeSpan _tokenExpirationTime;

    public TokenStorage(IConnectionMultiplexer connectionMultiplexer)
    {
        _redisDatabase = connectionMultiplexer.GetDatabase();
        _tokenExpirationTime = TimeSpan.FromDays(1);
    }

    public async Task AddTokenAsync(string token, string userId)
    {
        await _redisDatabase.StringSetAsync(token, userId, _tokenExpirationTime);
    }

    public async Task<bool> IsTokenActiveAsync(string token)
    {
        return !await _redisDatabase.KeyExistsAsync(token);
    }
}
