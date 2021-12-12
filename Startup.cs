global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http;
global using System.Text;
global using System.Text.Json;
global using System.Threading;
global using System.Threading.Tasks;
global using NwnServerStatus.Models;
global using NwnServerStatus.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NwnServerStatus.Startup))]
namespace NwnServerStatus
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<NwMasterService>();
        }
    }
}