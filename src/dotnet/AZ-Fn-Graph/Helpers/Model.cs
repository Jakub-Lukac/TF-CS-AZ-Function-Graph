using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZ_Fn_Graph.Helpers
{
    public class Model
    {
        public string tenantId { get; set; }
        [JsonIgnore]
        public string appId { get; set; }
        [JsonIgnore]
        public string appSecret { get; set; }

        public Model()
        {
            tenantId = Environment.GetEnvironmentVariable("CONF_TENANT_ID");
            appId = Environment.GetEnvironmentVariable("CONF_APP_ID");
            appSecret = Environment.GetEnvironmentVariable("CONF_APP_SECRET");
        }
    }
}
