using WasmBaseServices.Services.Contracts;

namespace WasmBaseServices.Services
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
            await _TokenService.DeleteLocalTokenAsync();
            await _TokenService.DeleteLocalRefreshTokenAsync();

            RemoveAuthTokenFromHttpHeader();
        }
    }
}