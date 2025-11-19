using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AuthRCL;

public class RclAuthStateProvider : AuthenticationStateProvider
{
    private readonly Services.RclAuthService _auth;

    public RclAuthStateProvider(Services.RclAuthService auth)
    {
        _auth = auth;
        _auth.AuthStateChanged += _ => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity = _auth.IsAuthenticated
            ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }, "FakeAuth")
            : new ClaimsIdentity();

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }
}
