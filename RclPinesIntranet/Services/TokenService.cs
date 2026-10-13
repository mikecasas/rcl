using Blazored.LocalStorage;
using Radzen.Blazor;
using RclPinesIntranet.Config;
using RclPinesIntranet.Services.Contracts;

namespace RclPinesIntranet.Services;

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
        string TokName = _Config.LocalStorageTokenName;

        Console.WriteLine($"Getting Token: {TokName}");
        return await GetLocalTokenAsync(TokName);
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        string TokName = _Config.LocalStorageRefreshTokenName;  

        Console.WriteLine($"Getting Refresh Token: {TokName}");
        return await GetLocalTokenAsync(TokName);
    }

    private async Task<string> GetLocalTokenAsync(string tokenName)
    {
        string tok = String.Empty;

        try
        {           
           tok = await _LocalStorage.GetItemAsync<string>(tokenName);

        }catch(Exception ex)
        {
            string s = ex.Message;
            Console.WriteLine($"Error: {s}");
        }

        return tok;
    }

    public async Task DeleteLocalTokenAsync()
    {
        await DeleteTokenAsync(_Config.LocalStorageTokenName);
    }

    public async Task DeleteLocalRefreshTokenAsync()
    {
        await DeleteTokenAsync(_Config.LocalStorageRefreshTokenName);
    }
    private async Task DeleteTokenAsync(string tokenName)
    {
        await _LocalStorage.RemoveItemAsync(tokenName);
    }
    public async Task<bool> IsTokenAsync()
    {
        var tok = await GetTokenAsync();
        return tok != null;
    }
    public async Task<bool> IsTokenExpiredAsync()
    {
        var tok = await GetTokenAsync();
        return IsTheTokenExpired(tok);
    }

    public async Task<bool> IsRefreshTokenExpiredAsync()
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

    public bool IsTokenExpired(string Token)
    {
        return IsTheTokenExpired(Token);
    }

    /// <summary>
    /// Gets the UserId from the token claims.
    /// </summary>
    /// <returns>Claim needs to be 'UserId'</returns>
    public async Task<string> GetUserIdAsync()
    {
        var Tok = await GetTokenAsync();

        if (Tok == null)
            return "";

        var Claims = JwtParser.ParseClaimsFromJwt(Tok);

        var TheUser = Claims.Where(claim => claim.Type.Equals("UserId")).FirstOrDefault();

        if (TheUser is null)
        {
            return string.Empty;
        }
        else
        {
            return TheUser.Value;
        }
    }
}