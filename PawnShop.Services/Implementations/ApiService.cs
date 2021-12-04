using PawnShop.Exceptions;
using PawnShop.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PawnShop.Services.Implementations
{
    public class ApiService : IApiService
    {
        public async Task<T> GetResponseInJson<T>(string uri) where T : class
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentException("Value cannot be null or empty.", nameof(uri));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(uri));

            try
            {
                return await TryToGetResponseInJson<T>(uri);
            }
            catch (Exception e)
            {
                throw new RequestApiException("Wystapil blad podczas zadania.", e);

            }
        }

        private async Task<T> TryToGetResponseInJson<T>(string uri) where T : class
        {
            using var httpClient = new HttpClient();
            var streamTask = httpClient.GetStreamAsync(uri);
            return await JsonSerializer.DeserializeAsync<T>(await streamTask);
        }
    }
}