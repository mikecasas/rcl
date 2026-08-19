namespace RclPinesIntranet.Config
{
    public interface IServiceBaseConfig
    {          
        string? LocalStorageRefreshTokenName { get; set; }
        string? LocalStorageTokenName { get; set; }    
    }
}