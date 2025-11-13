namespace RclPinesIntranet.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task<string> GetRefreshTokenAsync();
        Task<bool> IsToken();
        Task<bool> IsTokenExpired();
        Task<bool> IsRefreshTokenExpired();
        Task DeleteLocalToken();
        Task DeleteLocalRefreshToken();
    }
}
