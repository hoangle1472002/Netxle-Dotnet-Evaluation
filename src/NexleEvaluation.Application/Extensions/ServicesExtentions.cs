using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using NexleEvaluation.Application.Services.Implementations.Auths;
using NexleEvaluation.Application.Services.Interfaces.Auths;

namespace NexleEvaluation.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateHandler.GenerateErrorResponse;
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        }
    }
}
