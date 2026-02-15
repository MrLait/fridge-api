using Fridge.Api.ExceptionHandling;
using Fridge.Application;
using Fridge.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApiExceptionHandling()
    .AddCustomSwagger()
    .AddCustomAuth(builder.Configuration)
    .AddLocalStorage(builder.Configuration);

var app = builder.Build();

// Middleware
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDatabaseAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();