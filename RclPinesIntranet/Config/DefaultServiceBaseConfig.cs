
namespace RclPinesIntranet.Config
{
    public class DefaultServiceBaseConfig : ServiceBaseConfig
    {
        public DefaultServiceBaseConfig(AuthExtra authExtra)
        {
            this.IsDevelopment = true;
            this.UseDevEndpoints = true;
            this.UseLocalAuth = true;
            this.LocalAuthUrl = "https://tws.ppines.com/localhost-token/windows-auth/login-localhost";
            this.LocalStorageRefreshTokenName = authExtra.TokenNameRefresh;
            this.LocalStorageTokenName = authExtra.TokenNameJwt;
            this.FakeTenantNbr = 75;
            this.BaseUrl = string.Empty;
            this.LocalAuthSecret = "gideon";
        }
    }
}