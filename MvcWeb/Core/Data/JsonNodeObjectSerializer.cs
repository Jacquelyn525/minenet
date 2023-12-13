using System.Data;
using System.Text.Json.Nodes;

using Insight.Database;

namespace MvcWeb.Core.Data {
  public class JsonNodeObjectSerializer : DbObjectSerializer {
    public static readonly JsonNodeObjectSerializer Serializer = new();

    public override bool CanSerialize(Type type, DbType dbType) {
      return base.CanSerialize(type, dbType)
          && (type == typeof(JsonObject) || type == typeof(JsonArray));
    }

    public override object SerializeObject(Type type, object value) {
      if (value == null) {
        return null;
      }

      return ((JsonNode)value).ToJsonString();
    }

    public override bool CanDeserialize(Type sourceType, Type targetType) {
      return base.CanDeserialize(sourceType, targetType) &&
          (targetType == typeof(JsonObject) || targetType == typeof(JsonArray));
    }

    public override object DeserializeObject(Type type, object encoded) {
      if (encoded == null) {
        return null;
      }

      return JsonNode.Parse((string)encoded);
    }
  }
}
