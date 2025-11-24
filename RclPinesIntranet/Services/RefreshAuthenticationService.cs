using Blazored.LocalStorage;
using RclPinesIntranet.Config;
using RclPinesIntranet.Models;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services
{
    public class RefreshAuthenticationService : ServiceBase, IRefreshAuthenticationService
    {
        private ITokenService _TokenService;

        public RefreshAuthenticationService(HttpClient httpClient, ITokenService tokenService, UrlLocations urlLocations) : base(httpClient, tokenService, urlLocations)
        {
            _TokenService = tokenService;
        }

        public async Task<LoginResult> RefreshToken(string userId)
        {
            var RefreshRequest = new RefreshRequest();

            var RefreshTok = await _TokenService.GetRefreshTokenAsync();

            RefreshRequest.UserId = userId;
            RefreshRequest.RefreshToken = RefreshTok;

            Console.WriteLine(RefreshRequest);

            return await base.PostJsonAsync<RefreshRequest, LoginResult>("windows-auth/refresh-token", RefreshRequest);
        }
     
        private class RefreshRequest
        {
            public string? UserId { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}