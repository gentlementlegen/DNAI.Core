﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

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
        public string login;
        public string password;
    }

    internal class Token
    {
        //public string access_token;
        //public string expires_in;
        //public string token_type;
        //public string scope;
        //public string refresh_token;

        public string token;
        public string refreshToken;
        public string user_id;
}

    internal class File
    {
        public string _id;
        public string Title;
        public string Description;
        public string Type;
        public List<string> Paths;
        public List<string> Owners;
        //public bool in_store;
        //public string file;
        //public User user;
        //public User owner;
        //public FileType file_type;
        //public DateTime created;
        //public DateTime updated;
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
        [JsonIgnore]
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