namespace RclPinesIntranet.Config
{
    public interface IServiceBaseConfig
    {
        string? BaseUrl { get; set; }
        bool IsDevelopment { get; set; }      
        string? LocalStorageRefreshTokenName { get; set; }
        string? LocalStorageTokenName { get; set; }    
    }
}