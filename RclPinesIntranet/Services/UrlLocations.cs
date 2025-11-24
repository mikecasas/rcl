
namespace RclPinesIntranet.Services
{
    public class UrlLocations
    {
        private readonly string _login;
        private readonly string _api;

        public UrlLocations(string loginEndpoint, string apiBaseUrl)
        {
            _login = loginEndpoint;
            _api = apiBaseUrl;
        }

        public string LoginEndpoint { get => _login; }
        public string ApiUrlBase { get => _api; }
    }
}