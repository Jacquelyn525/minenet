namespace MvcWeb.Core.Data {
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class DatabaseServiceAttribute : Attribute {
    public DatabaseServiceAttribute(string connectionStringName) {
      ConnectionStringName = connectionStringName;
    }

    public string ConnectionStringName { get; }
  }

  public sealed class ConnectionStrings {
    public const string SqLiteDb = "SqLiteDbConnectionString";
    public const string MsSqlDb = "MsSqlDbConnectionString";
  }
}
