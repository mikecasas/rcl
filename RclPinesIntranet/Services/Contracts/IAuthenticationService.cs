
using RclPinesIntranet.Models;

namespace RclPinesIntranet.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(int tenantNbr);

        Task<LoginResult> FakeLogin(string secretKey);

        Task Logout();
    }
}