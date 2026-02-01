using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.ExceptionHandling;

public sealed class FallbackExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception ex) => true;

    public ProblemDetails Map(Exception ex) => new()
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Server error",
        Detail = "An expected error occurred."
    };
}