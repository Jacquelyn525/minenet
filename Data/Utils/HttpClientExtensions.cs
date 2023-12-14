using System.Text.Json;
using System.Xml.Serialization;

using MvcWeb;

namespace System.Net.Http {
  public static class HttpClientExtensions {
    public static async Task<T> ReadAs<T>(this HttpContent content) {
      var jsonStream = await content.ReadAsStreamAsync();
      if (jsonStream.Length > 0) {
        return JsonSerializer.Deserialize<T>(jsonStream, Constants.DefaultSerializerOptions);
      }

      return default;
    }

    public static async Task<T> ReadAsXml<T>(this HttpContent content) {
      var xmlStream = await content.ReadAsStreamAsync();
      if (xmlStream.Length > 0) {
        return (T)new XmlSerializer(typeof(T)).Deserialize(xmlStream);
      }
      return default;
    }
  }
}
