using Core.Plugin.Unity.Extensions;
using Newtonsoft.Json;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
namespace Core.Plugin.Unity.API
{
    /// <summary>
    /// Api accessor for Duly Web API.
    /// </summary>
    internal class ApiAccess
    {
        // http://163.5.84.173/
        private const string ApiAddress = "http://163.5.84.173/";

        private const string FilePath = "cloud/files/";
        private const string SolutionPath = "solution/";
        private const string UserPath = "users/";
        private const string AuthenticationPath = "oauth/token/";

        private readonly Accessor _accessor = new Accessor(ApiAddress);

        /// <summary>
        /// Retrieves a file using its Id.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        internal Task<File> GetFile(uint fileId)
        {
            return _accessor.GetObject<File>($"{FilePath}{fileId}");
        }

        /// <summary>
        /// Posts a file online.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal async Task PostFile(File file)
        {
            await _accessor.PostObject(FilePath, file);
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
                username = username,
                password = password,
                grant_type = "password"
            };
            var auth = "W8GMGSaFU71AVN6AXzQBxt68jQiNu5Gx5S7BmuMR:GCR0imL6Wx0sJk6qo8P7DuG0tQeEZUTxMNNbEnTrJO52T1tfA6FwyaNRxDWetMHTi6XXKT8tae1Ymxs299n4dF6s5OFYt4arU835PYgGnalDAN3aN5A0cZyG3HGibPsB".ToBase64();
            var msg = await _accessor.PostObjectEncoded(AuthenticationPath, user, auth);
            return JsonConvert.DeserializeObject<Token>(await msg.Content.ReadAsStringAsync());
        }
    }
}