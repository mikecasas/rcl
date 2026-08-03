using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using RclPinesIntranet.Config;
using RclPinesIntranet.Layout;
using RclPinesIntranet.Services;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBaseServices(this IServiceCollection services, string loginEndpoint, string apiBaseUrl, AuthExtra authExtra)
        {
            services.AddBlazoredLocalStorage();
            services.AddSingleton(new UrlLocations(loginEndpoint, apiBaseUrl));
            services.AddScoped<AuthExtra, AuthExtra>(sp => authExtra);
            services.AddScoped<IServiceBaseConfig>(sp => new DefaultServiceBaseConfig(new AuthExtra()));
            services.AddScoped<IServiceBase, ServiceBase>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

            services.AddScoped<LayoutState>();
            services.AddScoped<Helper>();

            services.AddAuthorizationCore();
            services.AddRadzenComponents();

            services.AddScoped<Notifications>();
            services.AddScoped<ConfigService>();
        }


        public static void AddCustomAuthenticationServices(this IServiceCollection services, bool useProjectAuth)
        {

            if(useProjectAuth)
            {
                services.AddScoped<IAuthenticationService, AuthenticationService>();
                services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            }
                
                





            ////Overide the default authentication service with the one provided by the caller
            //if (authenticationService is not null)
            //{
            //    services.AddScoped<IAuthenticationService>(sp => authenticationService);
            //}
            //else
            //{
            //    services.AddScoped<IAuthenticationService, AuthenticationService>();
            //}
        }



        public static void AddLibraryServices(this IServiceCollection services, string loginEndpoint, string apiBaseUrl
            ,AuthenticationStateProvider authenticationStateProvider, IAuthenticationService authenticationService
            ,AuthExtra authExtra)
        {           
            //services.AddBlazoredLocalStorage();
            //services.AddSingleton(new UrlLocations(loginEndpoint, apiBaseUrl));

            //services.AddScoped<AuthExtra, AuthExtra>(sp => authExtra);

            //services.AddScoped<IServiceBaseConfig>(sp => new DefaultServiceBaseConfig(authExtra));
            //services.AddScoped<IServiceBase, ServiceBase>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

            //services.AddScoped<LayoutState>();
            //services.AddScoped<Helper>();

            //Overide the default authentication service with the one provided by the caller
            if (authenticationService is not null)
            {
                services.AddScoped<IAuthenticationService>(sp => authenticationService);
            }
            else
            {
                services.AddScoped<IAuthenticationService, AuthenticationService>();
            }

            //Overide the default authentication service with the one provided by the caller
            if (authenticationService is not null)
            {
                services.AddScoped<AuthenticationStateProvider>(sp => authenticationStateProvider);
            }
            else
            {
                services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            }

            //services.AddAuthorizationCore();
            //services.AddRadzenComponents();         

            //Utility classes for code reuse to inject into Blazor pages
            //services.AddScoped<Notifications>();
            //services.AddScoped<ConfigService>();
        }
    }
}