using RclPinesIntranet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RclPinesIntranet.Services
{
    public class Helper : ServiceBase
    {
        private ITokenService _TokenService;

        public Helper(HttpClient client, ITokenService tokenService, UrlLocations urlLocations) : base(client, tokenService, urlLocations)
        {
            _TokenService = tokenService;

        }

        public async Task CleanUpOnLogoutAsync()
        {
            //Delete the tokens
            await _TokenService.DeleteLocalToken();
            await _TokenService.DeleteLocalRefreshToken();

            RemoveAuthTokenFromHttpHeader();
        }
    }
}