using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace RclPinesIntranet
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLibraryServices(this IServiceCollection services)
        {
            //services.AddSingleton<WeatherForecastService>();                 

            //services.AddScoped<IServiceBaseConfig, DefaultServiceBaseConfig>();
            //services.AddScoped<IServiceBase, ServiceBase>();
            //services.AddScoped<ITokenService, TokenService>();

            //services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

            //services.AddBlazoredLocalStorage();

            //services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            //services.AddAuthorizationCore();

            //services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddRadzenComponents();

            //services.AddScoped<DialogService>();
            //services.AddScoped<NotificationService>();
            //services.AddScoped<TooltipService>();
            //services.AddScoped<ContextMenuService>();

            //Utility classes for code reuse to inject into Blazor pages
            //services.AddScoped<Notifications>();
            //services.AddScoped<ConfigService>();

        }
    }
}