using Microsoft.Extensions.Configuration;

namespace RclPinesIntranet.Services
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration configuration)
        {
            config = configuration;
        }
    }
}