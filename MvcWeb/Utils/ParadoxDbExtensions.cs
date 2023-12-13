using System.Reflection;

namespace System.Data.OleDb {
  public static class OleDbDataReader {

    #region Helpers
    private static PropertyInfo[] colProps(PropertyInfo column) => column.GetType().GetProperties();
    private static ParadoxDbColumnAttribute? dbColAttribute(PropertyInfo column) => column.GetType().GetCustomAttribute<ParadoxDbColumnAttribute>();

    private static bool isModel<T>() => typeof(T).GetCustomAttribute<ParadoxDbModelAttribute>() != null;
    //private static bool isModel<T>() => typeof(T).GetType().GetCustomAttribute<ParadoxDbModelAttribute>() != null;
    //private static bool isCol(PropertyInfo column) => (column.GetType().GetCustomAttribute<ParadoxDbColumnAttribute>() != null);

    private static bool isCol(PropertyInfo column) => (column.GetCustomAttribute<ParadoxDbColumnAttribute>() != null);
    private static string getColName(PropertyInfo col) {
      var dbCol = col.GetCustomAttribute<ParadoxDbColumnAttribute>();
      return dbCol.ColumnName;
    }
    #endregion

    private static T getDefault<T>(T defaultValue = default!) => default!;

    public static object GetDefault(Type type) {
      if (type.GetTypeInfo().IsValueType) {
        return Activator.CreateInstance(type);
      }
      return null;
    }

    public static T ReadAsParadoxModel<T>(this IDataReader reader) {
      if (isModel<T>()) {
        var instance = (T)Activator.CreateInstance(typeof(T));
        if (instance != null) {
          var props = instance.GetType().GetProperties();

          foreach (var column in props) {
            if (isCol(column)) {
              var colName = getColName(column) != null ? getColName(column) : string.Empty;

              var ColType = column.PropertyType;

              var idx = reader.GetOrdinal(colName);

              var value = reader.GetValue(idx);
              var defaultValue = GetDefault(ColType);
              column.SetValue(instance, value.GetType() != typeof(DBNull) ? value : defaultValue);
            }
          }
          return instance;
        }
      }

      throw new Exception();
    }

    public static T GetParadoxColumn<T>(this IDataReader r, T column, T defaultValue = default) {
      var columnProps = column.GetType().GetCustomAttribute<ParadoxDbColumnAttribute>();

      if (columnProps != null) {
        var obj = r[columnProps.ColumnName];

        if (obj != null) {
          return (T)obj;
        }
      }

      return defaultValue;
    }
  }
}
