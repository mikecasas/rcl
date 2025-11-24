using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RclPinesIntranet;
using WebApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Assume this the Budget Narrative app, so pass in the api for that app. The Budget apps have an issue as the urls are not per tenant!!!!

builder.Services.AddLibraryServices("https://tws.ppines.com/saas-admin/windows-auth/login", "https://tws.ppines.com/pines-budget-metrics");

await builder.Build().RunAsync();