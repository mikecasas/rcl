using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddBlazoredLocalStorage();

            services.AddScoped<IServiceBaseConfig, DefaultServiceBaseConfig>();
            services.AddScoped<IServiceBase, ServiceBase>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddAuthorizationCore();

            services.AddRadzenComponents();         

            //Utility classes for code reuse to inject into Blazor pages
            services.AddScoped<Notifications>();
            services.AddScoped<ConfigService>();
        }
    }
}