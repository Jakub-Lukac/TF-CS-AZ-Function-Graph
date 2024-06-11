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
using System.Diagnostics;
using System.Reflection.Metadata;

namespace AZ_Fn_Graph
{
    public class GraphFunction
    {
        private readonly Code _code;

        public GraphFunction(Code code)
        {
            _code = code;
        }

        [FunctionName("GraphFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "default")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var parameters = new Model();

            /*var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);

            var user = await _code.GetUser(graphClient, "92ace2a0-26a3-4432-8254-ea8217fc2d8e");*/



            return new OkObjectResult(parameters.appId + "ha");
        }
    }
}
