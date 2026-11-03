using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using WasmBaseServices.Config;
using WasmBaseServices.Services;
using WasmBaseServices.Services.Contracts;

namespace RclPinesIntranet;

public static class ServiceCollectionExtensions
{
    public static void AddBaseServices(this IServiceCollection services, string loginEndpoint, string apiBaseUrl, AuthExtra authExtra)
    {
        Console.WriteLine($"Auth Extra Token Name: {authExtra.TokenName}");

        Console.WriteLine("Nuget Package Auth Extra Settings:");
        Console.WriteLine($"Token Audience: {authExtra.Audience}");
        Console.WriteLine($"Issuer: {authExtra.Issuer}");
        Console.WriteLine($"Token Name: {authExtra.TokenName}");


        services.AddBlazoredLocalStorage();
        services.AddSingleton(new UrlLocations(loginEndpoint, apiBaseUrl));
        services.AddScoped<AuthExtra, AuthExtra>(sp => authExtra);
        services.AddScoped<IServiceBaseConfig>(sp => new DefaultServiceBaseConfig(authExtra));
        services.AddScoped<IServiceBase, ServiceBase>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRefreshAuthenticationService, RefreshAuthenticationService>();

        //services.AddScoped<LayoutState>();
        services.AddScoped<Helper>();

        services.AddAuthorizationCore();
        services.AddRadzenComponents();

        services.AddScoped<Notifications>();
        services.AddScoped<ConfigService>();
    }

    public static void AddCustomAuthenticationServices(this IServiceCollection services, bool useBaseProjectAuth)
    {
        if(useBaseProjectAuth)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }              
    }       
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