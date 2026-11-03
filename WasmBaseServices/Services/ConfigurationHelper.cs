using Microsoft.Extensions.Configuration;

namespace WasmBaseServices.Services
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