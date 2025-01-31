using System.Reflection;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation;
using Presentation.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration)
);

// TODO: Add API title and description for Swagger
builder.Services.AddSwaggerGenWithAuth(title: "API Title", description: "API description");

builder
    .Services.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration, builder.Environment);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

await app.ApplyMigrations();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwaggerWithUi();

    await app.SeedData();
}

app.MapHealthChecks(
        "/health",
        new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
    )
    .CacheOutput(b => b.NoCache());

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseOutputCache();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
