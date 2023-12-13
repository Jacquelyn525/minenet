using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MvcWeb;
using MvcWeb.Core.Caching;

namespace Microsoft.AspNetCore.Builder {
  public static class ApplicationBuilderExtensions {
    public static void UseClientAppSettingsJs(this IApplicationBuilder app, object clientAppSettings) {
      var json = JsonSerializer.Serialize(clientAppSettings, Constants.DefaultSerializerOptions);
      var settingsJs = $"var config = {json}; window.__APP_CONFIG__ = Object.freeze(config)";

      app.Map("/settings.js", app => {
        app.Run(async context => {
          context.Response.ContentType = "text/javascript; charset=UTF-8";
          context.Response.AddNoCache();
          await context.Response.WriteAsync(settingsJs);
        });
      });
    }

    public static void UseRequestBuffering(this WebApplication app) {
      app.Use(async (context, next) => {
        // enable so WebApplicationExceptionHandler can read the body 
        context.Request.EnableBuffering();
        await next();
      });
    }

    public static void AddResponseCacheProfiles(this MvcOptions options, Settings settings) {
      options.CacheProfiles.Add(Constants.ResponseCacheProfiles.SystemData, new CacheProfile {
        Duration = CacheDurationHelper.GetTimeInSeconds(settings, CacheDuration.ResponseCacheSystemData),
        VaryByQueryKeys = new[] { "*" }
      });

      options.CacheProfiles.Add(Constants.ResponseCacheProfiles.Nonvolatile, new CacheProfile {
        Duration = CacheDurationHelper.GetTimeInSeconds(settings, CacheDuration.Nonvolatile),
        VaryByQueryKeys = new[] { "*" }
      });

      options.CacheProfiles.Add(Constants.ResponseCacheProfiles.Never, new CacheProfile { NoStore = true });
    }
  }
}
