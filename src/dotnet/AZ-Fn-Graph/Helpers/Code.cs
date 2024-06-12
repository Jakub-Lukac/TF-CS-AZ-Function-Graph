using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
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

        public async Task<List<User>> GetUsers(GraphServiceClient graphServiceClient, string groupID)
        {
            List<User> users = new List<User>();

            try
            {
                var userResult = await graphServiceClient.Groups[groupID].Members.GetAsync((requestConfiguration) => 
                {
                    requestConfiguration.QueryParameters.Top = 999;
                });

                users = users.Union(userResult.Value.Where(x => x.GetType() == typeof(User)).OrderBy(o => o.Id).Select(s => s as User).ToList()).ToList();

                var nextPageLink = userResult.OdataNextLink;
                while(nextPageLink != null)
                {
                    var nextPageRequestInformation = new RequestInformation
                    {
                        HttpMethod = Method.GET,
                        UrlTemplate = nextPageLink,
                    };

                    var nextPageResult = await graphServiceClient.RequestAdapter.SendAsync(nextPageRequestInformation, (parseNode) => new UserCollectionResponse());

                    users = users.Union(nextPageResult.Value.Where(x => x.GetType() == typeof(User)).OrderBy(o => o.Id).Select(s => s as User).ToList()).ToList();

                    nextPageLink = nextPageResult.OdataNextLink;
                }

                return users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<User>();
            }
        }

        public async Task<OkObjectResult> SendMail(GraphServiceClient graphServiceClient, string recipientEmail)
        {
            var requestBody = new SendMailPostRequestBody
            {
                Message = new Message
                {
                    Subject = "Exam results",
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Text,
                        Content = "Hi, the exams results are up.",
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = recipientEmail,
                            },
                        },
                    },
                    // cc - copy of the message
                    /*CcRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = "danas@contoso.com",
                            },
                        },
                    },*/
                },
                SaveToSentItems = false,
            };

            try
            {
                await graphServiceClient.Users["882b4e47-a1ba-4b54-8556-a85f852a0aa7"].SendMail.PostAsync(requestBody);
                return new OkObjectResult("Email sent successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new OkObjectResult("Error occured when sending an email.");
            }
        }
    }
}
