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
        private const string ApiAddress = "https://98c973e8-f9ef-4c95-9352-fd34fb61fd87.mock.pstmn.io/";

        private const string FilePath = "cloud/files/";
        private const string SolutionPath = "solution/";
        private const string UserPath = "users/";

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
            var url = UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor ? "software-windows/" : "software-mac/";
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
    }
}