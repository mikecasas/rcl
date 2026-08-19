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
 
    }
}