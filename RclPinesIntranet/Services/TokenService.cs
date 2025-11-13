using Blazored.LocalStorage;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService _localStorage;

        //Todo pull this from a config file
        public const string LocalStorageTokenName = "saas_admin_jwt";
        public const string LocalStorageRefreshTokenName = "saas_admin_refresh";


        public TokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            return await GetLocalToken(LocalStorageTokenName);
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            return await GetLocalToken(LocalStorageRefreshTokenName);
        }

        private async Task<string> GetLocalToken(string tokenName)
        {
            string tok = await _localStorage.GetItemAsync<string>(tokenName);

            if (string.IsNullOrWhiteSpace(tok))
                return null;

            return tok;
        }

        public async Task DeleteLocalToken()
        {
            await DeleteToken(LocalStorageTokenName);
        }

        public async Task DeleteLocalRefreshToken()
        {
            await DeleteToken(LocalStorageRefreshTokenName);
        }
        private async Task DeleteToken(string tokenName)
        {
            await _localStorage.RemoveItemAsync(tokenName);
        }
        public async Task<bool> IsToken()
        {
            var tok = await GetTokenAsync();
            return tok != null;
        }
        public async Task<bool> IsTokenExpired()
        {
            var tok = await GetTokenAsync();
            return IsTheTokenExpired(tok);
        }

        public async Task<bool> IsRefreshTokenExpired()
        {
            var tok = await GetRefreshTokenAsync();
            return IsTheTokenExpired(tok);
        }

        private bool IsTheTokenExpired(string tok)
        {
            if (tok == null)
                return true;

            var claims = JwtParser.ParseClaimsFromJwt(tok);

            // Checks the exp field of the token
            var expiry = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();

            if (expiry == null)
                return true;

            // The exp field is in Unix time
            var ExpirationDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
            var Nowwer = DateTime.UtcNow;

            if (ExpirationDateTime.UtcDateTime <= Nowwer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}