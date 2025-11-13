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
        private readonly AuthenticationStateProvider _AuthStateProvider;
        private ITokenService _TokenService;

        public AuthenticationService(HttpClient client, IServiceBaseConfig config, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider,
            ITokenService tokenService, ConfigService configService) : base(client, tokenService, configService)
        {
            _AuthStateProvider = authStateProvider;
            _LocalStorage = localStorage;
            _Config = config;
            _TokenService = tokenService;
        }

        public async Task<LoginResult> FakeLogin(string secretKey)
        {
            var Fk = new FakeTenantRequest { SecretKey = _Config.LocalAuthSecret };
            var loginResult = await PostJsonAsync<FakeTenantRequest, LoginResult>(_Config.LocalAuthUrl, true, Fk);

            if (loginResult == null || loginResult.IsSuccess == false)
            {
                return new LoginResult { IsSuccess = false };
            }

            if (loginResult == null || loginResult.IsSuccess == false)
            {
                return new LoginResult { IsSuccess = false };
            }

            return await AssignToken(loginResult);
        }

        public Task<LoginResult> Login(int tenantNbr)
        {
            throw new NotImplementedException();
        }

        public async Task Logout()
        {
            //Delete the tokens
            await _TokenService.DeleteLocalToken();
            await _TokenService.DeleteLocalRefreshToken();

            ((AuthStateProvider)_AuthStateProvider).NotifyUserLogout();
            RemoveAuthTokenFromHttpHeader();
        }

        private async Task<LoginResult> AssignToken(LoginResult loginResult)
        {
            if (loginResult != null && !string.IsNullOrEmpty(loginResult.AccessToken))
            {
                string Tok = loginResult.AccessToken;
                string RefreshTok = loginResult.RefreshToken;

                await _LocalStorage.SetItemAsync(_Config.LocalStorageTokenName, Tok);
                await _LocalStorage.SetItemAsync(_Config.LocalStorageRefreshTokenName, RefreshTok);
                base.AddAuthTokenToHttpHeader(Tok);

                ((AuthStateProvider)_AuthStateProvider).NotifyUserAuthentication(Tok);

                return new LoginResult { IsSuccess = true, DefaultPage = loginResult.DefaultPage };
            }
            else
            {
                return new LoginResult { IsSuccess = false };
            }
        }

        private TenantRequest GetTenantRequest(int tenantNbr)
        {
            var Request = new TenantRequest();

            if (_Config.UseLocalAuth)
            {
                Request.TenantNbr = _Config.FakeTenantNbr;
            }
            else
            {
                Request.TenantNbr = tenantNbr;
            }

            return Request;
        }

        private class TenantRequest
        {
            public int TenantNbr { get; set; }
        }

        private class FakeTenantRequest
        {
            public string SecretKey { get; set; }
        }
    }
}