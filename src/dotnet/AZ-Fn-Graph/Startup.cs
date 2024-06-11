using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AZ_Fn_Graph.Helpers;

[assembly: FunctionsStartup(typeof(AZ_Fn_Graph.Startup))]

namespace AZ_Fn_Graph
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<Code>();
        }
    }
}
