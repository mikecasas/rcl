using Microsoft.AspNetCore.Components.Authorization;
using RclPinesIntranet.Config;
using RclPinesIntranet.Services;
using RclPinesIntranet.Services.Contracts;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace RclPinesIntranet.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IRefreshAuthenticationService _AuthService;
        private ITokenService _TokenService;
        private readonly HttpClient _HttpClient;
        private readonly AuthExtra _AuthExtra;

        private const string AuthType = "jwtAuthType";

        public CustomAuthenticationStateProvider(HttpClient httpClient, ITokenService tokenService, IRefreshAuthenticationService authService
            , AuthExtra authExtra)
        {
            _HttpClient = httpClient;
            _TokenService = tokenService;
            _AuthService = authService;
            _AuthExtra = authExtra;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState Anonm = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var Token = await _TokenService.GetTokenAsync();
            var RefreshToken = await _TokenService.GetRefreshTokenAsync();

            if (Token == null || string.IsNullOrWhiteSpace(Token))
            {
                return Anonm;
            }

            var IsTokenExpired = await _TokenService.IsTokenExpiredAsync();
            if (IsTokenExpired)
            {
                return await HandleExpiredTokenAsync(Token, RefreshToken);
            }

            var AllClaimsExtracted = JwtParser.ParseClaimsFromJwt(Token);

            var IsValid = CheckAudianceAndIssuer(AllClaimsExtracted);

            if (IsValid) {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
                ClaimsIdentity identity = new ClaimsIdentity(AllClaimsExtracted, AuthType);
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                return Anonm;
            }
        }

        public void NotifyUserAuthentication(string token)
        {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthType));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            //Destroy the Tokens
            _TokenService.DeleteLocalTokenAsync();
            _TokenService.DeleteLocalRefreshTokenAsync();

            ClaimsIdentity identity = new ClaimsIdentity();
            var AuthState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(AuthState);
        }

        private async Task<AuthenticationState> HandleExpiredTokenAsync(string? token, string refreshToken)
        {
            AuthenticationState Anonm = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            Console.WriteLine("Token is Expired, so it was deleted.");
            await _TokenService.DeleteLocalTokenAsync();

            //For now, don't attempt to refresh the token, just log the user out. In the future, we can attempt to refresh the token if a refresh token is present.


            //Console.WriteLine("Checking the refresh token.....");

            //if (refreshToken is null || string.IsNullOrEmpty(refreshToken))
            //{
            //    //No refresh token, delete refresh token and start over!
            //    await _TokenService.DeleteLocalRefreshTokenAsync();
            //    return Anonm;
            //}
            //else
            //{
            //    var Claims = JwtParser.ParseClaimsFromJwt(token);
            //    var Claim = Claims.Where(x => x.Type == "UserId").FirstOrDefault();

            //    var RespRefresh = await _AuthService.RefreshToken(Claim.Value);

            //    if (RespRefresh is null)
            //    {
            //        Console.WriteLine("Refresh was not successful.");
            //        NotifyUserLogout();
            //        return Anonm;
            //    }
            //}
                        
            NotifyUserLogout();
            return Anonm;
        }

        private bool CheckAudianceAndIssuer(IEnumerable<Claim> claims)
        {
            //Any valid JWT will give the Blazor UI an authorization. This is a simple check to make sure the token is actually from our API
            //and not some random JWT from another source.

            Console.WriteLine($"Running...CheckAudianceAndIssuer");

            var aud = claims.Where(x => x.Type == "aud").FirstOrDefault();
            var iss = claims.Where(x => x.Type == "iss").FirstOrDefault();
            if (aud is null || iss is null)
            {
                Console.WriteLine($"aud claim is null or iss claim is null");
                return false;
            }              

            if (aud.Value == _AuthExtra.Audience && iss.Value == _AuthExtra.Issuer)
            {
                return true;
            }
            {
                return false;
            }            
        }
    }
}




//Refresh token example


//var Claims = JwtParser.ParseClaimsFromJwt(token);
//var Claim = Claims.Where(x => x.Type == "UserId").FirstOrDefault();

//var RespRefresh = await _AuthService.RefreshToken(Claim.Value);

//if (RespRefresh is null)
//{
//    Console.WriteLine("Refresh was not successful.");
//    NotifyUserLogout();
//    return Anonm;
//}









////return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));

////Works with micrsoft auth

//var claims = new List<Claim>
//{
//    new Claim(ClaimTypes.Name, "John Doe"),
//    new Claim(ClaimTypes.Role, "Administrator")
//};

//var anonymous = new ClaimsIdentity(claims, "testAuthType");
//return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));