using RclPinesIntranet.Services.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RclPinesIntranet.Services
{
    public class ServiceBase : IServiceBase
    {
        private readonly HttpClient _HttpClient;
        private ITokenService _TokenService;
        private readonly UrlLocations _UrlLocations;

        public ServiceBase(HttpClient httpClient, ITokenService tokenService, UrlLocations urlLocations)
        {
            _HttpClient = httpClient;
            _TokenService = tokenService;         
            _UrlLocations = urlLocations;
        }

        #region Get Requests

        public async Task<T> GetJsonAsync<T>(string uri)
        {
            return await GetJsonAsync<T>(uri, false);
        }

        public async Task<T> GetJsonAsync<T>(string uri, bool isfullUri, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Error:2000: The token passed into the method is null or empty.");
            }

            string FullUri = string.Empty;

            if (isfullUri)
            {
                FullUri = uri;
            }
            else
            {
                FullUri = FullUrl(uri);
            }
         
            Console.WriteLine($"Get Json: {uri}");

            var response = await GetHttpClient(token).GetAsync(FullUri, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);

            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                if(result is null)
                {
                    return default;
                } else
                {
                    return result;
                }               
            }

            return default;
        }

        public async Task<T> GetJsonAsync<T>(string uri, bool isfullUri)
        {
            var Tok = await _TokenService.GetTokenAsync();

            if (string.IsNullOrEmpty(Tok))
            {
                Console.WriteLine($"Error:8675309: Could not get a token from Local Storage.");               
            }

            return await GetJsonAsync<T>(uri, isfullUri, Tok);          
        }

        #endregion


        #region Post Requests

        public async Task<T> PostJsonAsync<T>(string uri, T value)
        {
            return await PostJsonAsync<T, T>(uri, value);
        }

        public async Task<TResult> PostJsonAsync<TValue, TResult>(string uri, TValue value)
        {
            return await PostJsonAsync<TValue, TResult>(uri, false, value);
        }

        public async Task<TResult> PostJsonAsync<TValue, TResult>(string uri, bool isFullUri, TValue value)
        {
            string FullUri = string.Empty;

            if (isFullUri)
            {
                FullUri = uri;
            }
            else
            {
                FullUri = FullUrl(uri);
            }

            var Tok = await _TokenService.GetTokenAsync();

            var response = await GetHttpClient(Tok).PostAsJsonAsync(FullUri, value);
            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                return result;
            }

            return default;
        }

        #endregion

        public void AddAuthTokenToHttpHeader(string token)
        {
            const string TokenAuth = "Bearer";

            if (!_HttpClient.DefaultRequestHeaders.Contains(TokenAuth))
            {
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenAuth, token);
            }
        }

        public void RemoveAuthTokenFromHttpHeader()
        {
            _HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<TResult> PostJsonWithHeadersAsync<TValue, TResult>(string uri, bool isFullUri, TValue value, Dictionary<string, string> headers)
        {          
            string FullUri = GetUrl(uri, isFullUri);

            var H = await GetHttpClientWithHeadersAsync(headers);

            var response = await H.PostAsJsonAsync(FullUri, value);

            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                return result;
            }

            return default;
        }

        public async Task<byte[]> GetByteArrayAsync(string uri)
        {
            return await GetByteArrayAsync(uri, false);
        }

        public async Task<byte[]> GetByteArrayAsync(string uri, bool fullUrl)
        {
            string FullUri = string.Empty;

            if (fullUrl)
            {
                FullUri = uri;
            }
            else
            {
                FullUri = FullUrl(uri);
            }

            var Tok = await _TokenService.GetTokenAsync();

            try
            {
                return await GetHttpClient(Tok).GetByteArrayAsync(FullUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return default;
        }


        #region Private Methods

        private async Task<HttpClient> GetHttpClientWithHeadersAsync(Dictionary<string, string> headers)
        {
            var Tok = await _TokenService.GetTokenAsync();

            var H = GetHttpClient(Tok);

            foreach (var header in headers)
            {
                H.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return H;
        }

        private string GetUrl(string uri, bool isFullUri)
        {
            string FullUri = string.Empty;

            if (isFullUri)
            {
                FullUri = uri;
            }
            else
            {
                FullUri = FullUrl(uri);
            }

            return FullUri;
        }


        private HttpClient GetHttpClient(string token)
        {
            const string Auth = "Bearer";

            //No token was passed in
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Error:100: The token passed in was null or empty.");
                return _HttpClient;
            }

            //Clear all the headers
            _HttpClient.DefaultRequestHeaders.Clear();

            if (!_HttpClient.DefaultRequestHeaders.Contains(Auth))
            {
                Console.WriteLine($"There is no Bearer token in the HTTP client");
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth, token);
            }

            return _HttpClient;
        }

        private string FullUrl(string partialUrl)
        {
           string? BaseUrl = _UrlLocations.ApiUrlBase;

            //Any login or refresh, send to Prod endpoint
            if (partialUrl.Contains("login"))
            {
                return _UrlLocations.LoginEndpoint;
            }
            return $"{BaseUrl}/{partialUrl}";
              
        }

        private bool ValidateJsonContent(HttpContent content)
        {
            var mediaType = content?.Headers.ContentType?.MediaType;
            return mediaType != null && mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;
            if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Request: {response.RequestMessage.RequestUri}");
                Console.WriteLine($"Response status: {response.StatusCode} {response.ReasonPhrase}");
            }

            return false;
        }

        public async Task<T> GetJsonWithHeadersAsync<T>(string uri, bool isFullUri, Dictionary<string, string> headers)
        {
            string FullUri = GetUrl(uri, isFullUri);

            var H = await GetHttpClientWithHeadersAsync(headers);

            var response = await H.GetAsync(FullUri, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);

            if (CheckResponse(response) && ValidateJsonContent(response.Content))
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            
            return default;
        }

        #endregion
    }
}