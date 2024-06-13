using System;
using System.Threading.Tasks;
using AZ_Fn_Graph.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AZ_Fn_Graph
{
    public class TimeTriggerFunc
    {
        private readonly ILogger _logger;
        private readonly Code _code;

        public TimeTriggerFunc(ILoggerFactory loggerFactory, Code code)
        {
            _logger = loggerFactory.CreateLogger<TimeTriggerFunc>();
            _code = code;
        }

        [Timeout("00:10:00")] // makes sure that the function does not run for longer than 10 minutes
        [Function("TimeTriggerFunc")]
        /* cron timers */
        // 0 */5 * * * *" == runs every time, the time is divisible by 5... => 50,55,00,05,...
        // 0 * * * * * == runs every minute, when the seconds is 00 ... => 11:27:00, 11:28:00,...
        // 2-5 13 * * * = runs every minute from 2 to 5, start at 13:02:00, and then goes every minute until 13:05:00
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var parameters = new Model();

            var graphClient = _code.GetAuthenticatedGraphClient(parameters.tenantId, parameters.appId, parameters.appSecret);

            await _code.SendMail(graphClient, "ChristieC@M365x25212640.OnMicrosoft.com", _logger);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
