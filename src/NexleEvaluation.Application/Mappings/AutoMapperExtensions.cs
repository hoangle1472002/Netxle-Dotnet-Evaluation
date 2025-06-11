using Microsoft.Extensions.DependencyInjection;

namespace NexleEvaluation.Application.Mappings
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapperProfilies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperExtensions).Assembly);
        }
    }
}
