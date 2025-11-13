using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RclPinesIntranet.Config
{
    public class ServiceBaseConfig : IServiceBaseConfig
    {
        public string? LocalStorageTokenName { get; set; }
        public string? LocalStorageRefreshTokenName { get; set; }
        public string? BaseUrl { get; set; }

        public string? LocalAuthUrl { get; set; }

        public bool IsDevelopment { get; set; }
        public bool UseDevEndpoints { get; set; }
        public bool UseLocalAuth { get; set; }
        public int FakeTenantNbr { get; set; }
        public string? LocalAuthSecret { get ; set; }
    }
}