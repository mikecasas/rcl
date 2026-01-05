using Microsoft.AspNetCore.Components.Authorization;
using RclPinesIntranet.Models;
using RclPinesIntranet.Services.Contracts;

namespace WebApp.LocalServices
{
    public class LocalAuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider _Local;

        public LocalAuthenticationService(AuthenticationStateProvider local)
        {
            _Local = local;
        }

        async Task<LoginResult> IAuthenticationService.LoginAsync(int tenantNbr)
        {
           await Task.Delay(500); // Simulate some delay

            //Generated from http://jwtbuilder.jamiekurtz.com/
            // Jwt key is 'password'

            ((LocalAuthenticationStateProvider)_Local).NotifyUserAuthentication(LocalKey.Tok);

            return new LoginResult { IsSuccess = true, DefaultPage = "blank" };
        }

        async Task IAuthenticationService.LogoutAsync()
        {
            await Task.Delay(500); // Simulate some delay

            var AuthThing = ((LocalAuthenticationStateProvider)_Local);
            AuthThing.NotifyUserLogout();
        }
    }
}
