using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

            ServicePointManager.ServerCertificateValidationCallback = MyValidationCallback;
        }

        /// <summary>
        /// This will check the SSL certificate to checks its validity.
        /// This has to be implemented because of Mono libraries shipped with Unity.
        /// See also https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool MyValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
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
        internal Task<HttpResponseMessage> PostObject<T>(string url, T @object, string typeHeader = "application/json")
        {
            var obj = JsonConvert.SerializeObject(@object);
            var buffer = System.Text.Encoding.UTF8.GetBytes(obj);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(typeHeader);
            return _client.PostAsync(url, byteContent);
        }

        /// <summary>
        /// Proceeds a POST call sending the given object in an Encoded Format.
        /// Optionnaly can take a string representing an Authorization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="object"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        internal Task<HttpResponseMessage> PostObjectEncoded<T>(string url, T @object)
        {
            var obj = JsonConvert.SerializeObject(@object);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj);
            var byteContent = new FormUrlEncodedContent(dic);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return _client.PostAsync(url, byteContent);
        }

        internal Task<HttpResponseMessage> PostObjectMultipart<T>(string url, T @object)
        {
            var json = JsonConvert.SerializeObject(@object);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var obj = new StringContent(json);
            var content = new MultipartFormDataContent();

            foreach (var field in dic)
            {
                content.Add(new StringContent(field.Value), field.Key);
            }

            return _client.PostAsync(url, content);
        }

        /// <summary>
        /// Proceeds a POST call for a Multipart Object, allowing binary data files to be uploaded.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="object"></param>
        /// <param name="FillAdditionalContent"></param>
        /// <returns></returns>
        internal Task<HttpResponseMessage> PostObjectMultipart<T>(string url, T @object, Action<MultipartFormDataContent> FillAdditionalContent)
        {
            var json = JsonConvert.SerializeObject(@object);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var obj = new StringContent(json);
            var content = new MultipartFormDataContent();

            foreach (var field in dic)
            {
                content.Add(new StringContent(field.Value), field.Key);
            }
            FillAdditionalContent?.Invoke(content);

            return _client.PostAsync(url, content);
        }

        internal Task<HttpResponseMessage> PutObjectMultipart<T>(string url, T @object, Action<MultipartFormDataContent> FillAdditionalContent)
        {
            var json = JsonConvert.SerializeObject(@object);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var obj = new StringContent(json);
            var content = new MultipartFormDataContent();

            foreach (var field in dic)
            {
                content.Add(new StringContent(field.Value), field.Key);
            }
            FillAdditionalContent?.Invoke(content);

            return _client.PutAsync(url, content);
        }

        internal Task<HttpResponseMessage> DeleteObject(string url)
        {
            return _client.DeleteAsync(url);
        }

        /// <summary>
        /// Sets an authorization using OAuth 2.0 and the given Token.
        /// </summary>
        /// <param name="token"></param>
        internal void SetAuthorizationOAuth(Token token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
        }

        /// <summary>
        /// Sets and authorization using Basic login.
        /// </summary>
        /// <param name="auth"></param>
        internal void SetAuthorizationBasic(string auth)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }
    }
}