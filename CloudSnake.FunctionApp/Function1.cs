using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudSnake.FunctionApp;

public class Function1
{
    [FunctionName("Function1")]
    public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    }
}