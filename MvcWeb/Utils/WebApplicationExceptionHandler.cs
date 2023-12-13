using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder {
  public class WebApplicationExceptionHandler {
    private const string _defaultErrorMessage = "An unexpected error occured on the server.";

    private readonly bool _showDetails;
    private readonly ILogger<WebApplicationExceptionHandler> _logger;

    public WebApplicationExceptionHandler(WebApplication app) {
      _showDetails = !app.Environment.IsEnvironment(Constants.Environments.Prod);
      _logger = app.Services.GetService<ILogger<WebApplicationExceptionHandler>>();
    }

    public async Task Invoke(HttpContext context) {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;

      var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
      if (ex == null) {
        return;
      }

      context.Response.ContentType = "application/problem+json";
      var problem = new ProblemDetails {
        Status = context.Response.StatusCode,
        Title = _showDetails ? ex.Message : _defaultErrorMessage,
        Detail = _showDetails ? ex.StackTrace : null
      };
      problem.Extensions["requestId"] = context.TraceIdentifier;
      problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

      var loggerScope = new Dictionary<string, object> {
        // RequestId is already logged
        ["TraceId"] = problem.Extensions["traceId"]
      };

      if (context.CanLogRequest()) {
        if (context.Request.Body.CanSeek && context.Request.ContentLength.GetValueOrDefault() > 0) {
          context.Request.Body.Position = 0;
          using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
          var body = await reader.ReadToEndAsync();
          context.Request.Body.Position = 0;
          loggerScope.Add("RequestBody", body);
        }
        if (context.Request.QueryString.HasValue) {
          loggerScope.Add("QueryString", context.Request.QueryString);
        }
      }

      foreach (DictionaryEntry de in ex.Data) {
        loggerScope.Add(de.Key.ToString(), de.Value);
      }

      // https://github.com/serilog/serilog-aspnetcore
      using (_logger.BeginScope(loggerScope)) {
        _logger.LogError(ex, "ExceptionHandler");
      }

      await JsonSerializer.SerializeAsync(context.Response.Body, problem);
    }
  }


}
