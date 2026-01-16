using Services;
using Services.Interfaces;
using Services.Security;

namespace WebApi
{
    public static class DependencyInjection
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IConsultantsService, ConsultantsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IRatesService, RatesService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
