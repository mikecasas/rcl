
namespace WasmBaseServices.Config;

public class DefaultServiceBaseConfig : ServiceBaseConfig
{
    public DefaultServiceBaseConfig(AuthExtra authExtra)
    {
        this.LocalStorageRefreshTokenName = authExtra.TokenNameRefresh;
        this.LocalStorageTokenName = authExtra.TokenNameJwt;
    }
}