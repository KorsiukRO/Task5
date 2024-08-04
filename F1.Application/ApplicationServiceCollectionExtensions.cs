// using F1.Application.Database;

using F1.Application.Database;
using F1.Application.Repositories;
using F1.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace F1.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<IRaceRepository, RaceRepository>();
        services.AddSingleton<ICarService, CarService>();
        services.AddSingleton<IRaceService, RaceService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}