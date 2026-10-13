# Pembroke Pines Intranet Razor Class Libary

3.1.0 - Adds GetUserIdAsync() to the token service to get the user id from the token.

3.0.0 - Breaking Changes

* Uses the lastest versions of Radzen (12.0.5) and .NET 10 (10.0.12)

https://blazor.radzen.com/get-started/wasm


* * Add this to the header for the theme to work

```sh
   <HeadContent>
    <RadzenTheme Theme="material" />
   </HeadContent>
```


2.0.0 - Breaking Changes

* New Auth model

```sh
    var AuthExtra = new RclPinesIntranet.Config.AuthExtra
    {
        Audience="cat",
        Issuer="dog",
        TokenName="some_token_name",
    };

    builder.Services.AddLibraryServices(UrlAuth, BaseUrl, null, null, AuthExtra);
```


* The nuget package for all intranet portals
* Common code for all intranet portals including:
    * Login/Logout
      * Authentication
    * Styling
        * Sidebars
        * Multiple Layouts
    * Error Handling
    * Common UI components




    * 
Below is sample code and styling
===


## Usage

1. In the Layout folder, create a layout wrapper control named `DefaultLayoutWrapper.razor`:

    ```sh
        @inherits LayoutComponentBase

    @using Microsoft.Extensions.Options
    @using TyingAccountsWasm.Models
    @inject IOptions<MySettings> Settings

    <RclPinesIntranet.Layout.PinesIntranet.MainLayout>

         <NavContent>
            <NavMenu />     
        </NavContent>

        <AuthorizationSection>
            <RclPinesIntranet.Components.AuthLinks TenantNbr="@Settings.Value.TenantNbr"></RclPinesIntranet.Components.AuthLinks>
        </AuthorizationSection>
      
        <HeaderContent>
            @* Don't need this in this app *@
            @*  <RclPinesIntranet.Layout.PinesIntranet.BudgetPortalSites BaseRoute="https://tws.ppines.com" HighlightLinkNbr="1" />  *@
        </HeaderContent>

        <Body>
            @Body
        </Body>

    </RclPinesIntranet.Layout.PinesIntranet.MainLayout>
    ```

2. In the `Program.cs` of the `API` project, add a reference via DI:

    ```sh
     bld.Services.AddSingleton<Tsd.Logger.IApplicationCommand, Tsd.Logger.AzureLogger>();
    ```

3. In the `Program.cs` file of the WASM project, add the following (change the namespace references):


    ```cs
        using Microsoft.AspNetCore.Components.Web;
        using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
        using TyingAccountsWasm.LocalServices;
        using RclPinesIntranet;
        using TyingAccountsWasm.Models;
        using TyingAccountsWasm;

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        string UrlAuth = "https://tws.ppines.com/saas-admin/windows-auth/login";

        string BaseUrl = builder.Configuration.GetSection("BaseUrl").Value ?? string.Empty;

        if (builder.HostEnvironment.IsProduction())
        {
            //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

            //Production Auth using the base class
            builder.Services.AddLibraryServices(UrlAuth, BaseUrl, null, null);
        }
        else
        {
            //builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);

            var L1 = new LocalAuthenticationStateProvider();
            var L2 = new LocalAuthenticationService(L1);

            //Use the local auth services for testing
            builder.Services.AddLibraryServices(UrlAuth, BaseUrl, L1, L2);
        }

        await builder.Build().RunAsync();

    ```

# Miscellaneous

* Bootstrap Styling

align-items == vertical align
justify-content == horizontal align