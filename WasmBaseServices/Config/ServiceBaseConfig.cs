
namespace WasmBaseServices.Config;

public class ServiceBaseConfig : IServiceBaseConfig
{
    public string? LocalStorageTokenName { get; set; }
    public string? LocalStorageRefreshTokenName { get; set; }

}