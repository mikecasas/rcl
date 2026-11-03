using WasmBaseServices.Models;

namespace WasmBaseServices.Services.Contracts
{
    public interface IRefreshAuthenticationService
    {      
        Task<LoginResult> RefreshToken(string userId);      
    }
}