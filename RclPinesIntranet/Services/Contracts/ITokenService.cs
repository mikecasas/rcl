namespace RclPinesIntranet.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task<string> GetRefreshTokenAsync();
        Task<bool> IsTokenAsync();
        Task<bool> IsTokenExpiredAsync();
        Task<bool> IsRefreshTokenExpiredAsync();
        Task DeleteLocalTokenAsync();
        Task DeleteLocalRefreshTokenAsync();
    }
}