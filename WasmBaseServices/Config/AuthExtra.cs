namespace WasmBaseServices.Config;

public class AuthExtra
{
    public string? TokenName { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }

    public string? TokenNameJwt { get=> GetJwtTokenName(); }
    public string? TokenNameRefresh { get => GetRefreshTokenName(); }

    private string GetJwtTokenName()
    {
        return string.IsNullOrEmpty(TokenName) ? "ERROR_JWT" : $"{TokenName}_jwt";
    }

    private string GetRefreshTokenName()
    {
        return string.IsNullOrEmpty(TokenName) ? "ERROR_REFRESH" : $"{TokenName}_refresh";
    }
}