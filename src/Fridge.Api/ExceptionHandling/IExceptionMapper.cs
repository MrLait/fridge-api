using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.ExceptionHandling;

public interface IExceptionMapper
{
    bool CanHandle(Exception exception);
    ProblemDetails Map(Exception ex);
}