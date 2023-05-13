using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RestaurantAggregator.Infra.Config;

namespace RestaurantAggregator.Infra;

public static class RabbitMqRegistration
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitConfiguration>(configuration.GetSection("RabbitConfig"));
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<RabbitConfiguration>>().Value;
            var factory = new ConnectionFactory()
            {
                HostName = config.Host,
                UserName = config.Username,
                Password = config.Password
            };
            return factory.CreateConnection();
        });
        return services;
    }
}
