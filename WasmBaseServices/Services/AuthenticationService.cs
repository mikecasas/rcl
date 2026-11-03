using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WasmBaseServices.Config;
using WasmBaseServices.Models;
using WasmBaseServices.Services.Contracts;

namespace WasmBaseServices.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        private readonly IServiceBaseConfig _Config;
        private readonly ILocalStorageService _LocalStorage;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;
        private ITokenService _TokenService;
        private readonly UrlLocations _UrlLocatations;

        public AuthenticationService(HttpClient client, IServiceBaseConfig config, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider,
            ITokenService tokenService, UrlLocations urlLocations) : base(client, tokenService, urlLocations)
        {
            _AuthenticationStateProvider = authenticationStateProvider;
            _LocalStorage = localStorage;
            _Config = config;
            _TokenService = tokenService;
            _UrlLocatations = urlLocations;
        }       

        public async Task<LoginResult> LoginAsync(int tenantNbr)
        {
            var TR = new TenantRequest { TenantNbr = tenantNbr };

            var LR = await PostJsonAsync<TenantRequest, LoginResult>(_UrlLocatations.LoginEndpoint, isFullUri: true, TR);

            if (LR is null || LR.IsSuccess == false)
            {
                return new LoginResult { IsSuccess = false };
            }

            return await AssignToken(LR);
        }

        public async Task LogoutAsync()
        {          
            var AuthThing = ((CustomAuthenticationStateProvider)_AuthenticationStateProvider);
            AuthThing.NotifyUserLogout();    
        }

        private async Task<LoginResult> AssignToken(LoginResult loginResult)
        {
            if (loginResult is not null && !string.IsNullOrEmpty(loginResult.AccessToken))
            {
                string Tok = loginResult.AccessToken;
                string RefreshTok = loginResult.RefreshToken;

                await _LocalStorage.SetItemAsync(_Config.LocalStorageTokenName, Tok);
                await _LocalStorage.SetItemAsync(_Config.LocalStorageRefreshTokenName, RefreshTok);
                base.AddAuthTokenToHttpHeader(Tok);

                ((CustomAuthenticationStateProvider)_AuthenticationStateProvider).NotifyUserAuthentication(Tok);

                return new LoginResult { IsSuccess = true, DefaultPage = loginResult.DefaultPage };
            }
            else
            {
                return new LoginResult { IsSuccess = false };
            }
        }      

        private class TenantRequest
        {
            public int TenantNbr { get; set; }
        }        
    }
}