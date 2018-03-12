using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
namespace Core.Plugin.Unity.API
{
    /// <summary>
    /// Api accessor for Duly Web API.
    /// </summary>
    internal class ApiAccess
    {
        /// <summary>
        /// Retrieves the currently set token for the connection.
        /// </summary>
        public Token Token { get; private set; }

        // http://163.5.84.173/
        private const string ApiAddress = "https://api.preprod.dnai.io/";

        private const string FilePath = "users/";
        private const string SolutionPath = "solution/";
        private const string UserPath = "users/";
        private const string AuthenticationPath = "signin/";

        private readonly Accessor _accessor = new Accessor(ApiAddress);

        /// <summary>
        /// Retrieves a file using its Id.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        internal Task<File> GetFile(string userId, string fileId)
        {
            return _accessor.GetObject<File>($"{FilePath}/{userId}/ias/{fileId}/");
        }

        /// <summary>
        /// Retrieves all the files currently present on the server.
        /// </summary>
        /// <returns></returns>
        internal async Task<List<File>> GetFiles(string userId)
        {
            var fileList = await _accessor.GetObject<List<File>>($"{FilePath}/{userId}/ias/");
            return fileList;
        }

        /// <summary>
        /// Posts a file online.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal Task<HttpResponseMessage> PostFile(FileUpload file)
        {
            return _accessor.PostObjectMultipart(FilePath, file, (content) =>
            {
                var fs = System.IO.File.ReadAllBytes(file.file);
                var sc = new ByteArrayContent(fs);
                content.Add(sc, nameof(file.file), System.IO.Path.GetFileName(file.file));
            });
        }

        internal Task<HttpResponseMessage> PutFile(FileUpload file)
        {
            return _accessor.PutObjectMultipart(FilePath, file, (content) =>
            {
                var fs = System.IO.File.ReadAllBytes(file.file);
                var sc = new ByteArrayContent(fs);
                content.Add(sc, nameof(file.file), System.IO.Path.GetFileName(file.file));
            });
        }

        internal Task<byte[]> GetFileContent(string userId, string fileId)
        {
            return _accessor.GetObjectRaw($"{FilePath}{userId}/ias/{fileId}/raw/");
        }

        internal Task<HttpResponseMessage> DeleteFile(string id)
        {
            return _accessor.DeleteObject(FilePath + id);
        }

        /// <summary>
        /// Opens a browser to download the DNAI solution.
        /// </summary>
        internal void DownloadSolution()
        {
            var url = ApiAddress;
            url += SolutionPath;
            url += UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor ? "software-windows/" : "software-mac/";
            UnityEngine.Debug.Log("opening url => " + url);
            UnityEngine.Application.OpenURL(SolutionPath + url);
        }

        /// <summary>
        /// Retrieves a user with the given id, if any.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal Task<User> GetUser(uint id)
        {
            return _accessor.GetObject<User>($"{UserPath}{id}/");
        }

        /// <summary>
        /// Gets an authentication token for the given user/password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal async Task<Token> GetToken(string username, string password)
        {
            var user = new UserToken
            {
                login = username,
                password = password
            };
            var msg = await _accessor.PostObject(AuthenticationPath, user);
            return JsonConvert.DeserializeObject<Token>(await msg.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Sets an OAuth authorization using OAuth 2.0.
        /// </summary>
        /// <param name="token"></param>
        internal void SetAuthorization(Token token)
        {
            _accessor.SetAuthorizationOAuth(token);
            Token = token;
        }
    }
}