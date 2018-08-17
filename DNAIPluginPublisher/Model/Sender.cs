using System;
using RestSharp;

// https://social.msdn.microsoft.com/Forums/en-US/0d0af0a8-e549-4451-914b-c296bd29fec4/convert-curl-command-to-c?forum=csharpgeneral
namespace DNAIPluginPublisher.Model
{
    /// <summary>
    /// Sends the package to the Website.
    /// </summary>
    public class Sender
    {
        public bool Send(string path, string login, string password)
        {
            return Send(path, login, password, null);
        }

        public bool Send(string path, string login, string password, Action onBeforeSend)
        {
            var client = new RestClient("https://api.dnai.io/signin");

            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Basic YWRtaW46ZG5haQ==");
            request.AddHeader("postman-token", "b3f8b8c8-fe13-7b8e-7aa7-ca49752db305");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", $"{{\"login\": \"{login}\", \"password\": \"{password}\"}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Log("Failed to log to the API.");
                return false;
            }
            Logger.Log("Successfully logged to the DNAI API.");
            return true;
        }
    }
}