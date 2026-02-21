using Fridge.Api.ExceptionHandling.Mappers;

namespace Fridge.Api.ExceptionHandling;

public static class ExceptionHandlingExtensions
{

    public static IServiceCollection AddApiExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddSingleton<IExceptionMapper, ValidationExceptionMapper>();
        services.AddSingleton<IExceptionMapper, NotFoundExceptionMapper>();
        services.AddSingleton<IExceptionMapper, UniqueConstraintExceptionMapper>();
        services.AddSingleton<IExceptionMapper, BusinessRuleViolationExceptionMapper>();
        services.AddSingleton<IExceptionMapper, FallbackExceptionMapper>();

        return services;
    }

}
