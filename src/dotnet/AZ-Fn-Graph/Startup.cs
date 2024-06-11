using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// this startup class would be startup of our function
[assembly: FunctionsStartup(typeof(AZ_Fn_Graph.Startup))]

namespace AZ_Fn_Graph
{
    // class for setting up the dependendcy injection
    // generally lifecycle starts here
    // confiure dependency injection container with our services and functions
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder) // == container
        {
            // place for configuring the dependency injection container

            // register my interface with concrete class (concrete == full implementation of the blueprint)

            builder.Services.AddLogging();
        }
    }
}
