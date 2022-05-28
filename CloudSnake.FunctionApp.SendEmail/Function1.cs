using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CloudSnake.FunctionApp.SendEmail;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> log)
    {
        _logger = log;
    }

    [FunctionName("Function1")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger logger)
    {
        logger.LogInformation($"SendEmailTimer executed at: {DateTime.Now}");

        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("johnny.hooyberghs@live.com", "Johnny Hooyberghs");
        var subject = "Sending with Twilio SendGrid is Fun";
        var to = new EmailAddress("johnny.hooyberghs@gmail.com", "Johnny Hooyberghs");
        var plainTextContent = "and easy to do anywhere, even with C#";
        var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
            
        return new OkObjectResult(htmlContent);
    }
}