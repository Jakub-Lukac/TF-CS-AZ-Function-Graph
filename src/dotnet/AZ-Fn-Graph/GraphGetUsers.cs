using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AZ_Fn_Graph.Helpers;
using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.Azure.Functions.Worker;
using System.Text;

namespace AZ_Fn_Graph
{
    public class GraphGetUsers
    {
        private readonly Code _code;
        private readonly ILogger _logger;

        public GraphGetUsers(Code code, ILogger<GraphGetUsers> logger)
        {
            _logger = logger;
            _code = code;
        }

        [Function("GraphGetUsers")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var parameters = new Model();

            var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);

            var users = await _code.GetUsers(graphClient, "08368be9-8784-4ddd-ac0e-13fc40fe5ccb");

            StringBuilder responseMessage = new StringBuilder();

            foreach (var u in users)
            {
                responseMessage.AppendLine($"{u.Id, -20}{u.DisplayName}");
            }

            return new OkObjectResult(responseMessage.ToString());
        }
    }
}
