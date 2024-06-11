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
    public class GraphGetUser
    {
        /*private readonly ILogger _logger;
        private readonly Code _code;

        public GraphGetUser(ILogger<GraphGetUser> logger, Code code)
        {
            _logger = logger;
            _code = code;
        }*/
        [FunctionName("GraphGetUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "default")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request");

            /*var parameters = new ParameterObjectBody();

            var graphClient = _code.GetAuthenticatedGraphClient(parameters.appId, parameters.appSecret, parameters.tenantId);

            var user = await _code.GetUser(graphClient, "2fe83868-254d-4787-ad17-00dbd91357b4");*/
            //var users = await _code.GetUsers(graphClient);

            /*StringBuilder responseMessage = new StringBuilder();

            foreach ( var user in users) 
            {
                responseMessage.AppendLine(user.DisplayName);
            }*/

            // e11.. - GraphTest app, run only when static keywords is used
            // takes local environment variable not the one from the azure portal

            // try to remove local conf_app_id and run the code, what will be the output
            // trouble shoot the static keyword, if it is needed, if no then the problem is with Graph Authentication
            string appId = Environment.GetEnvironmentVariable("CONF_APP_ID");

            return new OkObjectResult(appId);
        }
    }
}
