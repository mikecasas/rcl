using WasmBaseServices.Models;

namespace WasmBaseServices.Services.Contracts
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Method to login a user  
        /// </summary>
        /// <param name="tenantNbr"></param>
        /// <param name="loginEndpoint">The full url of the endpoint</param>
        /// <returns></returns>
        Task<LoginResult> LoginAsync(int tenantNbr);
       
        Task LogoutAsync();
    }
}