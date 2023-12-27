using System.Data.OleDb;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

using MvcWeb.Models.Configuration;
using MvcWeb.Models.MineNet;

using Serilog;

namespace MvcWeb.Paradox;

[SupportedOSPlatform("windows")]
[TransientService]
public class DbAdapter {
  private readonly Serilog.ILogger _log = Log.Logger;

  private readonly Settings _settings;


  // future maintainer:  I apologize for the following queries
  // this application and codebase was a rewrite of an earlier, which is where these queries come from, as-is.

  public DbAdapter(Settings settings) {
    _settings = settings;
  }

  private async Task<List<T>> ExecuteQuery<T>(string query, string dateFolder = null, string tagDbFile = null
    ) => await Task.Run(async () => {
      var builder = dateFolder == null
        ? _settings.MineNetConfig.DbConnStr()
        : _settings.MineNetConfig.HistConnStr(dateFolder, tagDbFile);

      var test = new OleDbConnection();

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
        _log.Error("{0}\n{1}", ex.Message, ex.StackTrace);
        throw;
      }

      connection.Close();

      return results;
    });

  public async Task<List<MinerEntry>> GetLocations() {

    var query = new StringBuilder();

    query.Append("Select t4.[Tag ID], t4.[MinerID], t4.[Last Name], t4.[First Name], t4.[Address], t4.[ZoneNumber], t4.[Zone], t4.[Reported], t4.[Signal Strength]");
    query.AppendLine(" From (Select [Tag ID], Max([Reported]) as Latest From [TagReader] Group By [Tag ID]) as t2 Inner Join (Select * From [TagReader] as t1");
    query.AppendLine(" Where Not Exists (Select * from [ExitZone] as t3 Where t1.[Address] = t3.[Address] And t1.[ZoneNumber] = t3.[ZoneNumber])) as t4");
    query.AppendLine(" On (t4.[Tag ID] = t2.[Tag ID]) And (t4.[Reported] = t2.[Latest]) Order By t4.[Last Name], t4.[First Name], t4.[Tag ID], t4.[Signal Strength] DESC");

    return await ExecuteQuery<MinerEntry>(query.ToString());
  }
  public async Task<List<AlertEntry>> GetAlerts() {

    var query = new StringBuilder();

    query.Append("Select [Address], [Device], [Type], [Alarm], [Occured], [Location], [Acknowledged], [Note]");
    query.AppendLine("FROM [Alarm]");

    return await ExecuteQuery<AlertEntry>(query.ToString());
  }

  #region Not Yet Implemented

  public async Task<List<MinerEntry>> GetEquipment() => await Task.Run(() => new List<MinerEntry>());
  public async Task<List<MinerEntry>> GetSupplyCars() => await Task.Run(() => new List<MinerEntry>());
  public async Task<List<MinerEntry>> GetTags() => await Task.Run(() => new List<MinerEntry>());

  #endregion
}
