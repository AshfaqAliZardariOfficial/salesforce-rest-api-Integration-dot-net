using System;
using System.Configuration;

namespace salesforce_rest_api_Integration_dot_net
{
    class Program
    {
        private static SalesforceClient CreateClient()
        {
            return new SalesforceClient
            {
                Username = ConfigurationManager.AppSettings["salesforce-username"],
                Password = ConfigurationManager.AppSettings["salesforce-password"],
                Token = ConfigurationManager.AppSettings["salesforce-token"],
                ClientID = ConfigurationManager.AppSettings["salesforce-clientid"],
                ClientSecret = ConfigurationManager.AppSettings["salesforce-clientsecret"],
            };
        }
        static void Main(string[] args)
        {
            var client = CreateClient();
            client.Login();
            Console.WriteLine();
            //Console.WriteLine(client.Query("select name from Account"));
            Console.WriteLine(client.Query("/services/data/", false ));
        }
    }
}
