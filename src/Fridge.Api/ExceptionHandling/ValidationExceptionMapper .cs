using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.ExceptionHandling;

public sealed class ValidationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception ex) => ex is ValidationException;

    public ProblemDetails Map(Exception ex)
    {
        var ve = (ValidationException)ex;

        var errors = ve.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation error"
        };
    }
}