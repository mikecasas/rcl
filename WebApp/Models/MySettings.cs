namespace WebApp.Models
{
    public class MySettings
    {
        public int TenantNbr { get; set; }
        public string? LoginEndpoint { get; set; }
     
        public string? CacheBust { get; set; }

        public bool ShowAccessToken { get; set; }       
    }
}