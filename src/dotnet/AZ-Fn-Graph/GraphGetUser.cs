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
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AZ_Fn_Graph
{
    public class GraphGetUser
    {
        private readonly ILogger<GraphGetUser> _logger;
        private readonly Code _code;

        public GraphGetUser(ILogger<GraphGetUser> logger, Code code)
        {
            _logger = logger;
            _code = code;
        }
        [FunctionName("GraphGetUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "default")] HttpRequest req)
        {
            DateTime messageDate = DateTime.Now;

            _logger.LogInformation($"C# Timer trigger function executed at: {messageDate}");

            /*var parameters = new ParameterObjectBody();

            var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);

            var users = await _code.GetUsers(graphClient);

            StringBuilder responseMessage = new StringBuilder();

            foreach (var u in users)
            {
                responseMessage.AppendLine($"{u.DisplayName, -20}{u.Id}");
            }*/

            return new OkObjectResult(messageDate);
        }
    }
}
