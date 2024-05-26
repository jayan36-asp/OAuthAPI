using OAuthAPI.Services.Interfaces;
using OAuthAPI.Services.Service;

namespace OAuthAPI.Services.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterServices (this IServiceCollection services)
        {
            return services.AddScoped<ILoginService, LoginService>();
        }
    }
}
