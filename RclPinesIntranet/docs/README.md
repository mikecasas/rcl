# Pembroke Pines Intranet Razor Class Libary

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

1. Install the Nuget package from the Pines repository by adding a reference to the `*.csproj` file of the `API` project in the solution:

    ```sh
     <PackageReference Include="Tsd.Logger" Version="3.2.0" />
    ```

2. In the `Program.cs` of the `API` project, add a reference via DI:

    ```sh
     bld.Services.AddSingleton<Tsd.Logger.IApplicationCommand, Tsd.Logger.AzureLogger>();
    ```

3. In the `Endpoint.cs` file of the Endpoint folder, add logic with the constructor:


    ```cs
      public class Endpoint : Endpoint<Request, Response>
      {

        private Tsd.Logger.IApplicationCommand _Logger;

        public Endpoint(Tsd.Logger.IApplicationCommand logger)
        {            
            _Logger = logger;
        }

          public override async Task HandleAsync(Request r, CancellationToken c)
          {
              _Logger.AddLog(Guid, UserName, ApplicationName, Message, Url);
              
              '''Other Code '''                          
            
          }

      }  

    ```