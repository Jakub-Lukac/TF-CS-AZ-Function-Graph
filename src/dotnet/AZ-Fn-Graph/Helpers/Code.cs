using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
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
        public GraphServiceClient GetAuthenticatedGraphClient(string clientId, string clientSecret, string tenantId)
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
            
            return new(clientSecretCredential, scopes);
        }

        public async Task<List<User>> GetUsers(GraphServiceClient graphServiceClient)
        {
            List<User> users = new List<User>();
            try
            {
                var usersResult = await graphServiceClient.Users.GetAsync((requestConfiguration) => 
                {
                    requestConfiguration.QueryParameters.Top = 999;
                    requestConfiguration.QueryParameters.Select = new string[] { "displayName" };
                });
                
                users = users.Union(usersResult.Value.Where(w => w.GetType() == typeof(User)).OrderBy(o => o.Id).Select(s => s as User).ToList()).ToList();

                var nextPageLink = usersResult.OdataNextLink;
                while (nextPageLink != null)
                {
                    var nextPageRequestInformation = new RequestInformation
                    {
                        HttpMethod = Method.GET,
                        UrlTemplate = nextPageLink,
                    };

                    var nextPageResult = await graphServiceClient.RequestAdapter.SendAsync(nextPageRequestInformation, (parseNode) => new DirectoryObjectCollectionResponse());
                    users = users.Union(nextPageResult.Value.Where(w => w.GetType() == typeof(User)).OrderBy(o => o.Id).Select(s => s as User).ToList()).ToList();
                    nextPageLink = usersResult.OdataNextLink;
                }

                return users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<User>();
            }
        }
    }
}
