﻿using System;
using RestSharp;

// https://social.msdn.microsoft.com/Forums/en-US/0d0af0a8-e549-4451-914b-c296bd29fec4/convert-curl-command-to-c?forum=csharpgeneral
namespace DNAIPluginPublisher.Model
{
    /// <summary>
    /// Sends the package to the Website.
    /// </summary>
    public class Sender
    {
        private const string SLUG = "installer";

        public bool Send(string path, string login, string password, Version version)
        {
            return Send(path, login, password, version, null);
        }

        public bool Send(string path, string login, string password, Version version, Action onBeforeSend)
        {
            IRestResponse response = ConnectToAPI(login, password);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Log("Failed to log to the API.");
                return false;
            }
            Logger.Log("Successfully logged to the DNAI API.");

            dynamic ds = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            string token = ds.token;
            response = CreateEntryAPI(login, password, token, version);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Log("Failed to create entry in the API.");
                return false;
            }
            Logger.Log("Successfully created entry in the DNAI API.");

            response = PatchEntryAPI(login, password, token, version);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Log("Failed to patched entry in the API.");
                return false;
            }
            Logger.Log("Successfully patched entry in the DNAI API.");

            onBeforeSend?.Invoke();
            response = UploadEntryAPI(login, password, token, version, path);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Log("Failed to upload entry in the API.");
                return false;
            }
            Logger.Log("Successfully uploaded entry in the DNAI API.");
            return true;
        }

        private IRestResponse ConnectToAPI(string login, string password)
        {
            var client = new RestClient("https://api.dnai.io/signin");

            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Basic YWRtaW46ZG5haQ==");
            request.AddHeader("postman-token", "b3f8b8c8-fe13-7b8e-7aa7-ca49752db305");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", $"{{\"login\": \"{login}\", \"password\": \"{password}\"}}", ParameterType.RequestBody);
            return client.Execute(request);
        }

        private IRestResponse CreateEntryAPI(string login, string password, string token, Version version)
        {
            var client = new RestClient("https://api.dnai.io/download/plugins/windows");

            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("postman-token", "4ba51ed8-cd69-7ddc-4dcf-2e78d3675e1e");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", $"{{\"title\": \"Revision for {version}\", \"description\": \"Generated by DNAI Plugin Publisher on Windows.\", \"currentVersion\": \"{version}\", \"slug\": \"{SLUG}\"}}", ParameterType.RequestBody);
            return client.Execute(request);
        }

        private IRestResponse PatchEntryAPI(string login, string password, string token, Version version)
        {
            var client = new RestClient("https://api.dnai.io/download/plugins/windows/" + SLUG);

            var request = new RestRequest(Method.PATCH);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("postman-token", "4ba51ed8-cd69-7ddc-4dcf-2e78d3675e1e");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", $"{{\"title\": \"Revision for {version}\", \"description\": \"Generated by DNAI Plugin Publisher on Windows.\", \"currentVersion\": \"{version}\", \"slug\": \"{SLUG}\"}}", ParameterType.RequestBody);
            return client.Execute(request);
        }

        private IRestResponse UploadEntryAPI(string login, string password, string token, Version version, string filePath)
        {
            var client = new RestClient("https://api.dnai.io/download/plugins/windows/" + SLUG + "/" + version);

            var request = new RestRequest(Method.PUT);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("postman-token", "c8638d15-6169-9fb9-2bb3-6314e0d66b45");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "text/plain");
            request.AddFile("DNAI Plugin", filePath);
            return client.Execute(request);
        }
    }
}