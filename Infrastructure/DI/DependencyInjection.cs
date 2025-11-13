using Domain.Repos;
using Infrastructure.Repos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICountryRepo, CountryRepo>();
        services.AddHostedService<TemporalBlockCleanupService>();
        // register other infra services here
        return services;
    }
}
