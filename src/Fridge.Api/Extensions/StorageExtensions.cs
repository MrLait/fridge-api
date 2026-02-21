using Fridge.Api.Options;
using Fridge.Application.Common.Interfaces;
using Fridge.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Fridge.Api.Extensions;

public static class StorageExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(
            configuration.GetSection(StorageOptions.SectionName));

        services.AddSingleton<IFileStorage>(sp =>
        {
            var env = sp.GetRequiredService<IHostEnvironment>();
            var opts = sp.GetRequiredService<IOptions<StorageOptions>>().Value;

            var root = Path.Combine(env.ContentRootPath, opts.RootPath);

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            return new LocalFileStorage(root);
        });

        return services;
    }
}