using System.Data.OleDb;

using MvcWeb.Models.Configuration;

namespace MvcWeb {
  
  [TransientService]
  public class ParadoxDbAdapter {
    private readonly Settings _settings;

    public ParadoxDbAdapter(Settings settings) {
      _settings = settings;
    }

    public async Task<List<T>> ExecuteQuery<T>(string query, string dateFolder = null, string tagDbFile = null) {
      return await Task.Run(async () => {
        var builder = dateFolder == null
          ? _settings.MineNetConfig.DbConnStr()
          : _settings.MineNetConfig.HistConnStr(dateFolder, tagDbFile);

        var connection = new OleDbConnection(builder.ConnectionString);
        var command = new OleDbCommand(query, connection);
        var results = new List<T>();

        connection.Open();
        try {
          await using (var reader = command.ExecuteReader()) {
            if (reader != null && reader.HasRows) {
              while (reader.Read()) {
                results.Add(reader.ReadAsParadoxModel<T>());
              }
            }
          }
        } catch (Exception ex) {
          // todo:  log and captureas
          throw;
        }

        connection.Close();
        return results;
      });
    }
  }
}
