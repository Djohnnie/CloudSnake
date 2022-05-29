using CloudSnake.DataAccess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudSnake.FunctionApp.Cleanup.Startup))]

namespace CloudSnake.FunctionApp.Cleanup;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddDbContext<CloudSnakeDbContext>();
    }
}