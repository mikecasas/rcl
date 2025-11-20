using Blazored.LocalStorage;
using RclPinesIntranet.Config;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService _LocalStorage;
        private readonly IServiceBaseConfig _Config;


        public TokenService(ILocalStorageService localStorage, IServiceBaseConfig config)
        {
            _LocalStorage = localStorage;
            _Config = config;
        }

        public async Task<string> GetTokenAsync()
        {
            return await GetLocalToken(_Config.LocalStorageTokenName);
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            return await GetLocalToken(_Config.LocalStorageRefreshTokenName);
        }

        private async Task<string> GetLocalToken(string tokenName)
        {
            string tok = String.Empty;

            try
            {
                tok = await _LocalStorage.GetItemAsync<string>(tokenName);
            }catch(Exception ex)
            {
                string s = ex.Message;               
            }

            return tok;
        }

        public async Task DeleteLocalToken()
        {
            await DeleteToken(_Config.LocalStorageTokenName);
        }

        public async Task DeleteLocalRefreshToken()
        {
            await DeleteToken(_Config.LocalStorageRefreshTokenName);
        }
        private async Task DeleteToken(string tokenName)
        {
            await _LocalStorage.RemoveItemAsync(tokenName);
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