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

            string confAppId = Environment.GetEnvironmentVariable("CONF_APP_ID");
            string confAppSecret = Environment.GetEnvironmentVariable("CONF_APP_SECRET");
            string confTenantId = Environment.GetEnvironmentVariable("CONF_TENANT_ID");
            string appInsightsKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");


            /*string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";*/

            string responseMessage = $"CONF TENANT ID : {confTenantId}";

            return new OkObjectResult(responseMessage);
        }
    }
}
