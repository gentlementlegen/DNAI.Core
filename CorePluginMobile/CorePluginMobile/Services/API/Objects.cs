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

    internal class File
    {
        public string _id;
        public string Title;
        public string Description;
        public string Type;
        public List<string> Paths;
        public List<string> Owners;
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