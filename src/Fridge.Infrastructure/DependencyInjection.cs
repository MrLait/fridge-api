using Fridge.Application.Common.Interfaces;
using Fridge.Infrastructure.Persistence;
using Fridge.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fridge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext>(x => x.GetRequiredService<AppDbContext>());
        services.AddScoped<DbSeeder>();
        services.AddScoped<IFridgeProductService, FridgeProductService>();
        services.AddScoped<IRestockService, RestockService>();
        services.AddScoped<IProductImageService, ProductImageService>();

        return services;
    }
}