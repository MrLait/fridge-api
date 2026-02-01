using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Api.ExceptionHandling;

public sealed class UniqueConstraintExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception ex) =>
        ex is DbUpdateException dbu && dbu.InnerException is SqlException sqlEx
        && (sqlEx.Number == 2601 || sqlEx.Number == 2627);

    public ProblemDetails Map(Exception ex) => new()
    {
        Status = StatusCodes.Status409Conflict,
        Title = "Conflict 409",
        Detail = "Unique constraint violation."
    };
}