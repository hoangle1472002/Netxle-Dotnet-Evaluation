using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Reflection;
using NexleEvaluation.API.Extensions;
using NexleEvaluation.API.Extensions.Middleware;
using NexleEvaluation.Application.Extensions;
using NexleEvaluation.Application.Mappings;
using NexleEvaluation.Application.Settings;
using NexleEvaluation.Infrastructure.Context;
using NexleEvaluation.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                                      .Enrich.FromLogContext()
                                      .CreateLogger();
builder.Host.UseSerilog(Log.Logger);
builder.Services.AddMemoryCache();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigurePersistenceApp();
builder.Services.ConfigureCors();
builder.Services.ConfigurateJWT(builder.Configuration);

builder.Services.ConfigureApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                            Assembly.GetExecutingAssembly(),
                            Assembly.Load("NexleEvaluation.Application")));

builder.Services.AddAutoMapperProfilies();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Swagger OpenAPI Configuration
var swaggerDocOptions = new SwaggerDocOptions();
builder.Configuration.GetSection(nameof(SwaggerDocOptions)).Bind(swaggerDocOptions);
builder.Services.ConfigureSwagger(swaggerDocOptions);

// Configure HTTP Strict Transport Security Protocol (HSTS)
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(1);
});

// Register and Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;

});

//-- Configure the HTTP request pipeline.
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();
