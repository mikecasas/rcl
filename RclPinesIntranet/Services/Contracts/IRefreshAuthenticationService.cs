using RclPinesIntranet.Models;

namespace RclPinesIntranet.Services.Contracts
{
    public interface IRefreshAuthenticationService
    {      
        Task<LoginResult> RefreshToken(string userId);      
    }
}