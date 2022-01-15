using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace salesforce_rest_api_Integration_dot_net
{
    class SalesforceClient
    {
        private const string LOGIN_ENDPOINT = "https://login.salesforce.com/services/oauth2/token";
        private const string API_ENDPOINT = "/services/data/v21.0/";
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }
        static SalesforceClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        public void Login()
        {
            //Console.WriteLine();
            //Console.WriteLine("Login Function Invoked");
            string jsonResponseString;
            using (var client = new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", ClientID },
                    { "client_secret", ClientSecret },
                    { "username", Username },
                    { "password", Password + Token },
                });
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.PostAsync(LOGIN_ENDPOINT, request).Result;
                jsonResponseString = response.Content.ReadAsStringAsync().Result;

            }
            Console.WriteLine();
            Console.WriteLine($"Response:{jsonResponseString}");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponseString);
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
            //Console.WriteLine();
            //Console.WriteLine("AuthToken=" + AuthToken);
            //Console.WriteLine();
            //Console.WriteLine("InstanceURL=" + InstanceUrl);
        } // Login function ends.

        public string Query(string soqlQuery, bool isQuery = false)
        {
            using var client = new HttpClient();
            string restRequest = InstanceUrl + API_ENDPOINT + (isQuery ? "query?q=" + soqlQuery : soqlQuery);
            var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
            request.Headers.Add("Authorization", "Bearer " + AuthToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-PreetyPrint", "1");
            var response = client.SendAsync(request).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
