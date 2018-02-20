using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
namespace Core.Plugin.API
{
    internal class User
    {
        internal string Username;
        internal string Password;
        internal string Email;
    }

    internal class File
    {
        internal uint Id;
        internal string Title;
    }

    /// <summary>
    /// Api accessor for Duly Web API.
    /// </summary>
    internal class ApiAccess
    {
        // http://163.5.84.173/
        private const string ApiAddress = "https://postman-echo.com/";

        private readonly HttpClient _client = new HttpClient();

        internal ApiAccess()
        {
            _client.BaseAddress = new Uri(ApiAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Retrieves a file using its Id.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        internal async Task<File> GetFile(uint fileId)
        {
            File file = null;
            HttpResponseMessage response = await _client.GetAsync("cloud/files");
            if (response.IsSuccessStatusCode)
            {
                file = JsonConvert.DeserializeObject<File>(await response.Content.ReadAsStringAsync());
            }
            return file;
        }

        /// <summary>
        /// Posts a file online.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal async Task PostFile(File file)
        {
            var obj = JsonConvert.SerializeObject(file);
            var buffer = System.Text.Encoding.UTF8.GetBytes(obj);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage msg = await _client.PostAsync($"cloud/files/{file.Id}", byteContent);
        }
    }
}