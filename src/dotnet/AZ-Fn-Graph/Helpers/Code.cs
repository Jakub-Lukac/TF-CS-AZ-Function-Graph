using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZ_Fn_Graph.Helpers
{
    public class Code
    {
        public GraphServiceClient GetAuthenticatedGraphClient(string tenantId, string clientId, string clientSecret)
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            return new(clientSecretCredential, scopes);
        }

        public async Task<User> GetUser(GraphServiceClient graphServiceClient, string userID)
        {
            try
            {
                var user = await graphServiceClient.Users[userID].GetAsync();
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
