using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.Plugin.Unity.API
{
    /// <summary>
    /// Class that helps with generic using of Http requests.
    /// </summary>
    internal class Accessor
    {
        public string ApiAddress { get; set; }

        private readonly HttpClient _client = new HttpClient();

        internal Accessor(string apiAddress)
        {
            ApiAddress = apiAddress;

            _client.BaseAddress = new Uri(ApiAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Proceeds a GET call using the given url to retrieve an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        internal async Task<T> GetObject<T>(string url)
        {
            T obj = default(T);
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                obj = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            return obj;
        }

        /// <summary>
        /// Proceeds a POST call sending the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        internal async Task PostObject<T>(string url, T @object)
        {
            var obj = JsonConvert.SerializeObject(@object);
            var buffer = System.Text.Encoding.UTF8.GetBytes(obj);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage msg = await _client.PostAsync(url, byteContent);
        }
    }
}