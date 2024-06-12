using AZ_Fn_Graph.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AZ_Fn_Graph
{
    public class GraphSendMail
    {
        private readonly ILogger<GraphSendMail> _logger;
        private readonly Code _code;

        public GraphSendMail(ILogger<GraphSendMail> logger, Code code)
        {
            _logger = logger;
            _code = code;
        }

        [Function("GraphSendMail")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP POST function executed.");

            var parameters = new Model();

            var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);

            return await _code.SendMail(graphClient, "AlexW@M365x25212640.OnMicrosoft.com");
        }
    }
}
