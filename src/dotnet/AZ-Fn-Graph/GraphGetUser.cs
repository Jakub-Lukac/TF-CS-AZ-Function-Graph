using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AZ_Fn_Graph.Helpers;
using System.Text;

namespace AZ_Fn_Graph
{
    public static class GraphGetUser
    {
        [FunctionName("GraphGetUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "default")] HttpRequest req, ILogger log, Code code)
        {
            log.LogInformation($"C# HTTP trigger function processed a request");

            var parameters = new ParameterObjectBody();

            var graphClient = code.GetAuthenticatedGraphClient(parameters.appId, parameters.appSecret, parameters.tenantId);

            var user = await code.GetUser(graphClient, "2fe83868-254d-4787-ad17-00dbd91357b4");
            //var users = await _code.GetUsers(graphClient);

            /*StringBuilder responseMessage = new StringBuilder();

            foreach ( var user in users) 
            {
                responseMessage.AppendLine(user.DisplayName);
            }*/

            return new OkObjectResult(user.DisplayName);
        }
    }
}
