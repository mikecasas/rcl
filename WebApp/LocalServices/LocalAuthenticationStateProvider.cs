using Microsoft.AspNetCore.Components.Authorization;
using RclPinesIntranet.Services;
using System.Security.Claims;

namespace WebApp.LocalServices;

public class LocalAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string AuthType = "jwtAuthType";
 
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Console.WriteLine($"LocalAuthenticationStateProvider.GetAuthenticationStateAsync()");    

        //var Identity2 = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }, "CustomAuth");

        var AllClaimsExtracted = JwtParser.ParseClaimsFromJwt(LocalKey.Tok);
        ClaimsIdentity Identity = new ClaimsIdentity(AllClaimsExtracted, AuthType);

        return new AuthenticationState(new ClaimsPrincipal(Identity));
    }

    public void NotifyUserAuthentication(string token)
    {
        Console.WriteLine($"LocalAuthenticationStateProvider.NotifyUserAuthentication()");

        //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthType));
        //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        //NotifyAuthenticationStateChanged(authState);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        Console.WriteLine($"LocalAuthenticationStateProvider.NotifyUserLogout()");

        ClaimsIdentity identity = new ClaimsIdentity();
        var AuthState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        NotifyAuthenticationStateChanged(AuthState);
    }
}