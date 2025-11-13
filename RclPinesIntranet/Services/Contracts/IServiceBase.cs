using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RclPinesIntranet.Services.Contracts
{
    public interface IServiceBase
    {
        Task<T> GetJsonAsync<T>(string uri);
        Task<T> GetJsonAsync<T>(string uri, bool isFullUri);

        Task<T> PostJsonAsync<T>(string uri, T value);
        Task<TResult> PostJsonAsync<TValue, TResult>(string uri, TValue value);
        Task<TResult> PostJsonAsync<TValue, TResult>(string uri, bool isFullUri, TValue value);
        Task<TResult> PostJsonWithHeadersAsync<TValue, TResult>(string uri, bool isfullUri, TValue value, Dictionary<string, string> headers);

        void RemoveAuthTokenFromHttpHeader();
        void AddAuthTokenToHttpHeader(string token);

        Task<byte[]> GetByteArrayAsync(string uri);
        Task<byte[]> GetByteArrayAsync(string uri, bool fullUrl);
    }
}