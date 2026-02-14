using Fridge.Infrastructure;
using Fridge.Application;
using Fridge.Api.ExceptionHandling;
using Fridge.Infrastructure.Persistence;
using Fridge.Infrastructure.Options;
using Fridge.Application.Common.Interfaces;
using Fridge.Infrastructure.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.SectionName));

builder.Services.AddSingleton<IFileStorage>(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var opts = sp.GetRequiredService<IOptions<StorageOptions>>().Value;

    var root = Path.Combine(env.ContentRootPath, opts.RootPath);

    Directory.CreateDirectory(root);
    return new LocalFileStorage(root);
});


builder.Services.AddApiExceptionHandling();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await dbSeeder.SeedAsync();
}

app.MapControllers();

app.Run();