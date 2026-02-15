using Fridge.Api.Constants;
using Microsoft.OpenApi.Models;

namespace Fridge.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(ApiConstants.Swagger.DocName, new OpenApiInfo
            {
                Title = ApiConstants.Swagger.Title,
                Version = ApiConstants.Swagger.Version
            });

            c.AddSecurityDefinition(ApiConstants.Swagger.BearerSchemeName, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = ApiConstants.Swagger.BearerScheme,
                BearerFormat = ApiConstants.Swagger.BearerFormat,
                In = ParameterLocation.Header,
                Description = ApiConstants.Swagger.BearerDescription
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApiConstants.Swagger.BearerSchemeName
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}