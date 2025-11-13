using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using RclPinesIntranet.Config;
using RclPinesIntranet.Services;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLibraryServices(this IServiceCollection services)
        {
            //services.AddSingleton<WeatherForecastService>();                 

            services.AddBlazoredLocalStorage();

            services.AddScoped<IServiceBaseConfig, DefaultServiceBaseConfig>();
            services.AddScoped<IServiceBase, ServiceBase>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            services.AddAuthorizationCore();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddRadzenComponents();         

            //Utility classes for code reuse to inject into Blazor pages
            services.AddScoped<Notifications>();
            services.AddScoped<ConfigService>();

        }
    }
}