
using WasmBaseServices.Models;

namespace RclPinesIntranet.Factories;

public class LoginResultFactory
{
    public static LoginResult GetSuccessLoginResult()
    {
        return new LoginResult { IsSuccess = true };
    }

    public static LoginResult GetSuccessLoginResult(string defaultPage)
    {

        return new LoginResult { IsSuccess = true, DefaultPage = defaultPage };
    }

    public static LoginResult GetFailureLoginResult()
    {

        return new LoginResult { IsSuccess = false };
    }
}