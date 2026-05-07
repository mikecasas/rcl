using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RclPinesIntranet.Config
{
    public class DefaultServiceBaseConfig : ServiceBaseConfig
    {
        public DefaultServiceBaseConfig(string tokenName)
        {
            string BaseTokenName = string.IsNullOrEmpty(tokenName) ? "ERROR_ppines" : $"{tokenName}_jwt";
            string RefreshTokenName = string.IsNullOrEmpty(tokenName) ? "ERROR_ppines" : $"{tokenName}_refresh";

            this.IsDevelopment = true;
            this.UseDevEndpoints = true;
            this.UseLocalAuth = true;
            this.LocalAuthUrl = "https://tws.ppines.com/localhost-token/windows-auth/login-localhost";
            this.LocalStorageRefreshTokenName = RefreshTokenName;
            this.LocalStorageTokenName = BaseTokenName;
            this.FakeTenantNbr = 75;
            this.BaseUrl = string.Empty;
            this.LocalAuthSecret = "gideon";
        }
    }
}