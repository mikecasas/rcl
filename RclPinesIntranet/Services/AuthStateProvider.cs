using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IRefreshAuthenticationService _AuthService;
        private ITokenService _TokenService;
        private readonly HttpClient _HttpClient;

        private const string AuthType = "jwtAuthType";
        	
		public AuthStateProvider(HttpClient httpClient, ITokenService tokenService, IRefreshAuthenticationService authService)
        {
            _HttpClient = httpClient;
            _TokenService = tokenService;
            _AuthService = authService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState Anonm = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var token = await _TokenService.GetTokenAsync();
            var RefreshToken = await _TokenService.GetRefreshTokenAsync();

            if (token == null || string.IsNullOrWhiteSpace(token))
            {
                return Anonm;
            }

            var IsTokenExpired = await _TokenService.IsTokenExpired();
            if (IsTokenExpired)
            {
                Console.WriteLine("Token is Expired, so it was deleted.");
                await _TokenService.DeleteLocalToken();
               
                Console.WriteLine("Checking the refresh token.....");
               
                if (RefreshToken == null || string.IsNullOrEmpty(RefreshToken))
                {
                    //No refresh token, delete refresh token and start over!
                    await _TokenService.DeleteLocalRefreshToken();
                    return Anonm;
                }
                else
                {
                    var Claims = JwtParser.ParseClaimsFromJwt(token);
                    var Claim = Claims.Where(x=> x.Type == "UserId").FirstOrDefault();

                    var RespRefresh = await _AuthService.RefreshToken(Claim.Value);

                    if (RespRefresh == null)
                    {
                        Console.WriteLine("Refresh was not successful.");
                        NotifyUserLogout();
                        return Anonm;
                    }                   
                }
            }

            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var AllClaimsExtracted = JwtParser.ParseClaimsFromJwt(token);

            ClaimsIdentity identity = new ClaimsIdentity(AllClaimsExtracted, AuthType);

            return new AuthenticationState(new ClaimsPrincipal(identity));
            ////return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));

            ////Works with micrsoft auth

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, "John Doe"),
            //    new Claim(ClaimTypes.Role, "Administrator")
            //};

            //var anonymous = new ClaimsIdentity(claims, "testAuthType");
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));

        }

        public void NotifyUserAuthentication(string token)
        {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthType));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            //NotifyAuthenticationStateChanged(authState);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            //Need to destroy the token in local storage!!!!!

            //var authState = Task.FromResult(_anonymous);
            //NotifyAuthenticationStateChanged(authState);

            //destroy the tokens
            _TokenService.DeleteLocalToken();
            _TokenService.DeleteLocalRefreshToken();

			ClaimsIdentity identity = new ClaimsIdentity();
            var AS = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(AS);
        }
    }
}
