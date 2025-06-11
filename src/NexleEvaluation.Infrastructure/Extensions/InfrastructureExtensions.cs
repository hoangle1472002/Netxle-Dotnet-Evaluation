using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using NexleEvaluation.Infrastructure.Context;
using NexleEvaluation.Infrastructure.Repositories;
using NexleEvaluation.Infrastructure.UnitOfWorks;
using NexleEvaluation.Domain.Interfaces;

namespace NexleEvaluation.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static void ConfigurePersistenceApp(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                            ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")))
                           .LogTo(Console.WriteLine, LogLevel.Information)
                           .EnableSensitiveDataLogging()
                           .EnableDetailedErrors();

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring SQL context: {ex.Message}");
                throw;
            }
        }
    }
}