using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.ExceptionHandling;

public sealed class NotFoundExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception ex) => ex is KeyNotFoundException;

    public ProblemDetails Map(Exception ex) => new()
    {
        Status = StatusCodes.Status404NotFound,
        Title = "Not found",
        Detail = ex.Message
    };
}