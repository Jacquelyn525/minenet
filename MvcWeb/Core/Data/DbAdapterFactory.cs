using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Data.SqlClient;

using Insight.Database;

namespace MvcWeb.Core.Data {
  public static class DbAdapterFactory {
    public static T GetConnectionAs<T>(string connectionString) where T : class {
      return new SqlConnectionStringBuilder(connectionString).As<T>();
    }

    static DbAdapterFactory() {
      Insight.Database.Providers.MsSqlClient.SqlInsightDbProvider.RegisterProvider();

      JsonObjectSerializer.Initialize();
      DbSerializationRule.Serialize<JsonObject>(JsonNodeObjectSerializer.Serializer);
      DbSerializationRule.Serialize<JsonArray>(JsonNodeObjectSerializer.Serializer);
      DbSerializationRule.Serialize<JsonElement>(JsonElementObjectSerializer.Serializer);
    }
  }
}
