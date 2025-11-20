using Blazored.LocalStorage;


namespace RclPinesIntranet.Services
{
    public abstract class ConfigServiceSettings
    {
        public string? ApiBaseUrl { get; protected set; }
        public string? ApplicationName { get; protected set; }
        public string? LoginUser { get; protected set; }
        public string? LoginPassword { get; protected set; }
        public string? LoginEndpoint { get; protected set; }

        public string? ConfigEndpoint { get; protected set; }
    }

    public class ConfigService : ConfigServiceSettings
    {   
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        private const string CacheKey = "AppConfig";
        private const string CacheKeyDts = "CacheCreatedOn";
        private const int CacheDeletionMinutes = 15;

        public ConfigService(HttpClient http, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _http = http;
        }       
    
        public async Task LoadConfigAsync()
        {    
            string JsonString = string.Empty;

            if (await _localStorage.ContainKeyAsync(CacheKey))
            {
                //Use cached version  
                //Get Cache created on date time stamp
                var CreatedOn = await _localStorage.GetItemAsync<DateTime>(CacheKeyDts);

                TimeSpan duration = DateTime.UtcNow -CreatedOn;
                double TotalMinutes = duration.TotalMinutes;

                if (TotalMinutes > CacheDeletionMinutes)
                {
                    //Clear cache and get new version
                    await ClearCache();
                    await GetConfigFromApiAsync();
                    return;
                }

                JsonString = await _localStorage.GetItemAsync<string>(CacheKey);
                ParseTheItems(JsonString);
            }
            else
            {
                await GetConfigFromApiAsync();
            }          
        }

        private async Task GetConfigFromApiAsync()
        {
            string BaseAddress = _http.BaseAddress.ToString();
            string ModifiedBaseAddress = string.Empty;

            //If there is a trailing slash, remove it
            if (BaseAddress.EndsWith("/"))
            {
                //Has trailing slash
                BaseAddress.Remove(BaseAddress.Length - 1);            
            }
            else
            {
                //No trailing slash
                ModifiedBaseAddress = BaseAddress;  
            }

            //Call the API to get config and store in local storage
            string ConfigUrl = ModifiedBaseAddress + ConfigEndpoint;  //All of this is becasue of docker, but it's good to get all config from the API. Better re-usability.

            try
            {
                var Config = await _http.GetAsync(ConfigUrl);

                string JsonString = await Config.Content.ReadAsStringAsync();

                await _localStorage.SetItemAsync(CacheKey, JsonString);
                await _localStorage.SetItemAsync(CacheKeyDts, DateTime.UtcNow);

                ParseTheItems(JsonString);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }          
        }

        private async Task ClearCache()
        {
            await _localStorage.RemoveItemAsync(CacheKey);
            await _localStorage.RemoveItemAsync(CacheKeyDts);
        }   

        private void ParseTheItems(string jsonString)
        {
            ApplicationName = ConfigParser.GetValueFromDefaultConfig(jsonString, "ApplicationName");
            ApiBaseUrl = ConfigParser.GetValueFromDefaultConfig(jsonString, "ApiBaseUrl");
            LoginUser = ConfigParser.GetValueFromDefaultConfig(jsonString, "LoginUser");
            LoginPassword = ConfigParser.GetValueFromDefaultConfig(jsonString, "LoginPassword");
            LoginEndpoint = ConfigParser.GetValueFromDefaultConfig(jsonString, "LoginEndpoint");
        }
    }
}