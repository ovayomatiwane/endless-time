using Services;
using Services.Interfaces;

namespace WebApi
{
    public static class DependencyInjection
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IConsultantsService, ConsultantsService>();
        }
    }
}
