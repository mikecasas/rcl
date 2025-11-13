using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RclPinesIntranet.Config
{
    public class DefaultServiceBaseConfig : ServiceBaseConfig
    {
        public DefaultServiceBaseConfig()
        {
            this.IsDevelopment = true;
            this.UseDevEndpoints = true;
            this.UseLocalAuth = true;
            this.LocalAuthUrl = "https://tws.ppines.com/localhost-token/windows-auth/login-localhost";
            this.LocalStorageRefreshTokenName = "ppines_budget_refresh";
            this.LocalStorageTokenName = "ppines_budget_jwt";
            this.FakeTenantNbr = 75;
            this.BaseUrl = string.Empty;
            this.LocalAuthSecret = "gideon";
        }
    }
}