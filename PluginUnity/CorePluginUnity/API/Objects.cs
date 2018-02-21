namespace Core.Plugin.Unity.API
{
    internal class User
    {
        public string Username;
        public string Password;
        public string Email;
    }

    internal class UserToken
    {
        public string grant_type;
        public string username;
        public string password;
    }

    internal class Token
    {
        public string access_token;
        public string expires_in;
        public string token_type;
        public string scope;
        public string refresh_token;
}

    internal class File
    {
        public uint Id;
        public string Title;
        public string Description;
    }
}