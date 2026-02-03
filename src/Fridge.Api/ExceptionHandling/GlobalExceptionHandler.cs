using Microsoft.AspNetCore.Diagnostics;

namespace Fridge.Api.ExceptionHandling;
public sealed class GlobalExceptionHandler(IEnumerable<IExceptionMapper> mappers) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var mapper = mappers.FirstOrDefault(m => m.CanHandle(exception));
        if (mapper is null)
            return false;

        var problem = mapper.Map(exception);

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}