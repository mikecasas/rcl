using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RclPinesIntranet;
using RclPinesIntranet.Services;
using RclPinesIntranet.Services.Contracts;
using WebApp;
using WebApp.LocalServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Assume this the Budget Narrative app, so pass in the api for that app. The Budget apps have an issue as the urls are not per tenant!!!!

string ApiBaseUrl = builder.Configuration.GetSection("ApiBaseUrl").Value ?? string.Empty;
string UrlAuth = builder.Configuration.GetSection("AuthUrl").Value ?? string.Empty;

var AuthExtra = new RclPinesIntranet.Config.AuthExtra();
builder.Configuration.GetSection("AuthSettings").Bind(AuthExtra);


builder.Services.AddBaseServices(UrlAuth, ApiBaseUrl, AuthExtra);


//Set the custom services for authentication, this will override the default services in the library. The library will use these services instead of the default ones.


builder.Services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();



builder.Services.AddCustomAuthenticationServices(false);

//if (builder.HostEnvironment.IsProduction())
//{
//    //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

//    //Production Auth using the base class
//    builder.Services.AddLibraryServices(UrlAuth, ApiBaseUrl, null, null, AuthExtra);
//}
//else
//{
//    //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);

//    // 2. Register your provider and pull both services from the provider
//    builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>(sp =>
//    {
//        var httpClient = sp.GetRequiredService<HttpClient>();
//        var tokenService = sp.GetRequiredService<ITokenService>();

//        var L1 = new LocalAuthenticationStateProvider(httpClient, tokenService);
//        var L2 = new LocalAuthenticationService(L1);

//        //Use the local auth services for testing
//        builder.Services.AddLibraryServices(UrlAuth, ApiBaseUrl, L1, L2, AuthExtra);

//        return L1;
//    });

//    //builder.Services.AddScoped<LocalAuthenticationService>();


//    //var L1 = new LocalAuthenticationStateProvider(builder.Services.BuildServiceProvider().GetService<HttpClient>(), builder.Services.BuildServiceProvider().GetService<TokenService>());
//    //var L2 = new LocalAuthenticationService(L1);


//}


//builder.Services.AddLibraryServices("https://tws.ppines.com/saas-admin/windows-auth/login", "https://tws.ppines.com/pines-budget-metrics", null, null);

await builder.Build().RunAsync();