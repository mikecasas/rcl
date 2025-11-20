using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RclPinesIntranet.Config;
using RclPinesIntranet.Models;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        private readonly IServiceBaseConfig _Config;
        private readonly ILocalStorageService _LocalStorage;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;
        private ITokenService _TokenService;

        public AuthenticationService(HttpClient client, IServiceBaseConfig config, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider,
            ITokenService tokenService, ConfigService configService) : base(client, tokenService, configService)
        {
            _AuthenticationStateProvider = authenticationStateProvider;
            _LocalStorage = localStorage;
            _Config = config;
            _TokenService = tokenService;
        }       

        public async Task<LoginResult> LoginAsync(int tenantNbr, string loginEndpoint)
        {
            var TR = new TenantRequest { TenantNbr = tenantNbr };

            var LR = await PostJsonAsync<TenantRequest, LoginResult>(loginEndpoint, isFullUri: true, TR);

            if (LR is null || LR.IsSuccess == false)
            {
                return new LoginResult { IsSuccess = false };
            }

            return await AssignToken(LR);
        }

        public async Task LogoutAsync()
        {
            //Delete the tokens
            await _TokenService.DeleteLocalToken();
            await _TokenService.DeleteLocalRefreshToken();

            var AuthThing = ((CustomAuthenticationStateProvider)_AuthenticationStateProvider);
            AuthThing.NotifyUserLogout();       
                        
            RemoveAuthTokenFromHttpHeader();
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