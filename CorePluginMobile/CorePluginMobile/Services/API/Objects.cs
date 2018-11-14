using System.Collections.Generic;

namespace CorePluginMobile.Services.API
{
    internal class User
    {
        public string Username;
        public string Password;
        public string Email;
    }

    internal class UserToken
    {
        public string login;
        public string password;
    }

    [System.Serializable]
    public class Token
    {
        public string token;
        public string refreshToken;
        public string user_id;

        /// <summary>
        /// Check if the token is currently empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(token) && string.IsNullOrEmpty(refreshToken);
        }
    }

    public class File
    {
        public string _id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<string> Paths { get; set; }
        public List<string> Owners { get; set; }
    }

    internal class FileType
    {
        public uint id;
        public string label;
        public string descirption;
    }

    internal class FileUpload
    {
        public uint file_type_id;
        public string title;
        public bool in_store;

        public string file;
    }

    internal class FileList
    {
        public uint count;
        public object next;
        public object previous;
        public List<File> results;
    }
}