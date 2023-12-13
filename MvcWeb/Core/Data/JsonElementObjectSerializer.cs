using System.Data;
using System.Text.Json;

using Insight.Database;

namespace MvcWeb.Core.Data {
  /// <summary>
  ///   Insight.Database helper, use with random blobs of Json
  ///   Helpful to stream raw Json to web
  ///   Serializes JsonElement to string
  ///   Deserializes string to JsonElement
  /// </summary>
  public class JsonElementObjectSerializer : DbObjectSerializer {
    public static readonly JsonElementObjectSerializer Serializer = new();

    public override bool CanSerialize(Type type, DbType dbType) {
      return base.CanSerialize(type, dbType) && type == typeof(JsonElement);
    }

    public override object SerializeObject(Type type, object value) {
      if (value == null) {
        return null;
      }

      return value.ToString();
    }

    public override bool CanDeserialize(Type sourceType, Type targetType) {
      return base.CanDeserialize(sourceType, targetType) && targetType == typeof(JsonElement);
    }

    public override object DeserializeObject(Type type, object encoded) {
      if (encoded == null) {
        return null;
      }

      return JsonDocument.Parse((string)encoded).RootElement.Clone();
    }
  }
}
