namespace RclPinesIntranet.Config
{
    public interface IServiceBaseConfig
    {
        string? BaseUrl { get; set; }
        bool IsDevelopment { get; set; }
        string? LocalAuthUrl { get; set; }
        string? LocalAuthSecret { get; set; }
        string? LocalStorageRefreshTokenName { get; set; }
        string? LocalStorageTokenName { get; set; }
        bool UseDevEndpoints { get; set; }
        bool UseLocalAuth { get; set; }

        int FakeTenantNbr { get; set; }
    }
}