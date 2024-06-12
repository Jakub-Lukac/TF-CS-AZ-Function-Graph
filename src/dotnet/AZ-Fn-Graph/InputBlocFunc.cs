using System.IO;
using System.Threading.Tasks;
using AZ_Fn_Graph.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AZ_Fn_Graph
{
    public class InputBlocFunc
    {
        private readonly ILogger<InputBlocFunc> _logger;
        private readonly Code _code;

        public InputBlocFunc(ILogger<InputBlocFunc> logger, Code code)
        {
            _logger = logger;
            _code = code;
        }

        [Function(nameof(InputBlocFunc))]
        public async Task Run([BlobTrigger("blob-func-container/{name}", Connection = "")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");

            var parameters = new Model();
            var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);
            await _code.SendMail(graphClient, "ChristieC@M365x25212640.OnMicrosoft.com", name, content);
        }
    }
}
