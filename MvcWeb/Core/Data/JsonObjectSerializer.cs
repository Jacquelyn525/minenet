using System.Data;
using System.Text.Json;

using Insight.Database;

namespace MvcWeb.Core.Data {
  public class JsonObjectSerializer : DbObjectSerializer {
    readonly JsonSerializerOptions options = Constants.DefaultSerializerOptions;
    public static void Initialize() {
      Insight.Database.JsonObjectSerializer.OverrideSerializer = new JsonObjectSerializer();
    }

    public override bool CanSerialize(Type type, DbType dbType) {
      return base.CanSerialize(type, dbType) || dbType == DbType.Object;
    }

    public override object DeserializeObject(Type type, object encoded) {
      return JsonSerializer.Deserialize((string)encoded, type, options);
    }

    public override object SerializeObject(Type type, object value) {
      if (value == null) {
        return null;
      }

      return JsonSerializer.Serialize(value, type, options);
    }
  }
}
