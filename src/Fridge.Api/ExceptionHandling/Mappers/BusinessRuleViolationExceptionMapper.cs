using Fridge.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.ExceptionHandling.Mappers;

public sealed class BusinessRuleViolationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is BusinessRuleViolationException;

    public ProblemDetails Map(Exception ex) => new()
    {
        Status = StatusCodes.Status422UnprocessableEntity,
        Title = "Business rule violation",
        Detail = ex.Message
    };
}