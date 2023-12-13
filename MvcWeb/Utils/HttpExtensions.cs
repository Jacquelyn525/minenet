using Microsoft.Net.Http.Headers;

using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.AspNetCore.Http {
  public static class HttpExtensions {
    public static void AddNoCache(this HttpResponse response) {
      response.Headers[HeaderNames.CacheControl] = "no-cache, no-store";
      response.Headers[HeaderNames.Pragma] = "no-cache";
      //response.Headers[HeaderNames.Expires] = "-1";
    }

    public static (string, string) GetBasicAuthCredentials(this HttpRequest request) {
      if (!AuthenticationHeaderValue.TryParse(request.Headers[HeaderNames.Authorization], out var header) || string.IsNullOrEmpty(header.Parameter)) {
        return (null, null);
      }
      var values = Encoding.UTF8
          .GetString(Convert.FromBase64String(header.Parameter))
          .Split(':', 2);

      if (values == null || values.Length < 2) {
        return (null, null);
      }

      return (values[0], values[1]);
    }

    private const string _canLogRequestKey = nameof(CanLogRequest);

    /// <summary>
    /// Opt out of request logging
    /// </summary>
    /// <param name="httpContext"></param>
    public static void DoNotLogRequest(this HttpContext httpContext) => httpContext.Items[_canLogRequestKey] = false;

    public static bool CanLogRequest(this HttpContext httpContext) => httpContext.Items[_canLogRequestKey] as bool? ?? true;
  }
}
