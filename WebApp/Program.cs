using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RclPinesIntranet;
using WebApp;
using WebApp.LocalServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Assume this the Budget Narrative app, so pass in the api for that app. The Budget apps have an issue as the urls are not per tenant!!!!

string BaseUrl = builder.Configuration.GetSection("BaseUrl").Value ?? string.Empty;
string UrlAuth = builder.Configuration.GetSection("AuthUrl").Value ?? string.Empty;

var AuthExtra = new RclPinesIntranet.Config.AuthExtra
{
    Audience="cat",
    Issuer="dog",
    TokenName="some_token_name",
};

builder.Services.AddLibraryServices(UrlAuth, BaseUrl, null, null, AuthExtra);


//if (builder.HostEnvironment.IsProduction())
//{
//    //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

//    //Production Auth using the base class
//    builder.Services.AddLibraryServices(UrlAuth, BaseUrl, null, null, AuthExtra);
//}
//else
//{
//    //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);

//    var L1 = new LocalAuthenticationStateProvider();
//    var L2 = new LocalAuthenticationService(L1);

//    //Use the local auth services for testing
//    builder.Services.AddLibraryServices(UrlAuth, BaseUrl, L1, L2, AuthExtra);
//}

//builder.Services.AddLibraryServices("https://tws.ppines.com/saas-admin/windows-auth/login", "https://tws.ppines.com/pines-budget-metrics", null, null);

await builder.Build().RunAsync();