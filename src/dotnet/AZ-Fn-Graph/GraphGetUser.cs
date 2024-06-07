using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AZ_Fn_Graph
{
    public static class GraphGetUser
    {
        [FunctionName("GraphGetUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "default")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string name = req.Query["name"];
                log.LogInformation($"Name from query: {name}");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Request Body: {requestBody}");

                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;
                log.LogInformation($"Name after body processing: {name}");

                string responseMessage = string.IsNullOrEmpty(name)
                    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                    : $"Hello, {name}. This HTTP triggered function executed successfully.";

                log.LogInformation("Function executed successfully.");
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
