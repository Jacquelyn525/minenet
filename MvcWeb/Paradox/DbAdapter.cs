using System.Data.OleDb;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

using MvcWeb.Models;
using MvcWeb.Models.Configuration;
using MvcWeb.Models.MineNet;

using Serilog;

namespace MvcWeb.Paradox;

[SupportedOSPlatform("windows")]
[TransientService]
public class DbAdapter {

  #region Setup

  private readonly Serilog.ILogger _log = Log.Logger;

  private readonly Settings _settings;


  // future maintainer:  I apologize for the following queries
  // this application and codebase was a rewrite of an earlier, which is where these queries come from, as-is.

  public DbAdapter(Settings settings) {
    _settings = settings;
  }

  #endregion

  #region Private Nuts and Bolts

  private async Task<List<T>> ExecuteQuery<T>(string query, string dateFolder = null, string tagDbFile = null) {
    return await Task.Run(async () => {
      var builder = dateFolder == null
        ? _settings.MineNetConfig.DbConnStr()
        : _settings.MineNetConfig.HistConnStr(dateFolder, tagDbFile);

      var test = new OleDbConnection();

      var connection = new OleDbConnection(builder.ConnectionString);
      var command = new OleDbCommand(query, connection);
      var results = new List<T>();

      connection.Open();

      try {
        await using (var reader = await command.ExecuteReaderAsync()) {
          if (reader != null && reader.HasRows) {
            while (await reader.ReadAsync()) {
              results.Add(reader.ReadAsParadoxModel<T>());
            }
          }
        }
      } catch (Exception ex) {
        _log.Error("{0}\n{1}", ex.Message, ex.StackTrace);
        //throw;
      }

      connection.Close();

      return results;
    });
  }

  //static async Task<List<string>> ReadDatabaseAsync(string filePath) {
  //  // Read data from the database file asynchronously
  //  var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={filePath};Integrated Security=True";
  //  using (var connection = new SqlConnection(connectionString)) {
  //    await connection.OpenAsync();

  //    using (var command = new SqlCommand("SELECT * FROM YourTable", connection))
  //    using (var reader = await command.ExecuteReaderAsync()) {
  //      var result = new List<string>();

  //      while (await reader.ReadAsync()) {
  //        // Process each row as needed
  //        result.Add(reader.GetString(0)); // Assuming a string column, adjust accordingly
  //      }

  //      return result;
  //    }
  //  }
  //}

  #endregion

  #region History

  // lets be honest - this is really just the same thing as executequery - so can probably deprecate and remove.
  private async Task<List<TagIdEntry>> queryParadoxDb(string query, string datePath, string timePath) {
    var entries = await ExecuteQuery<TagIdEntry>(query, datePath, timePath);
    //sendHubUpdate($"Read {entries.Count} records... Now attempting to insert");

    return entries;
  }

  private string HistoryQueryBase {
    get {
      var sb = new StringBuilder();
      sb.Append("SELEC");
      sb.AppendLine("[Tag ID],          [Address],        [ZoneNumber],     [DateKey],        [MinuteKey],");
      sb.AppendLine("[Last Name],       [First Name],     [Zone],           [Reported],       [Battery],");
      sb.AppendLine("[Signal Strength], [Message],        [Temperature],    [Source],         [Last Zone],");
      sb.AppendLine("[Last Reported],   [Rate Reported],  [Last Rate],      [Message Count],");
      sb.AppendLine("[Message Alarm],   [MinerID]");

      return sb.ToString();
    }
  }


  private void ReadArchivesAsync(List<ITagHistoryArchive> archives) {

  }


  public async Task<List<TagIdListData>> GetArchiveUniqueTagIds(ITagHistoryArchive path) {
    var sb = new StringBuilder();
    sb.Append("SELECT DISTINCT");
    sb.AppendLine("[Tag ID], [First Name], [Last Name], [MinerID]");
    sb.AppendLine($"FROM [{path.TimePath}]");
    sb.AppendLine("ORDER BY [Tag ID] ASC");

    return await ExecuteQuery<TagIdListData>(sb.ToString(), path.DatePath, path.TimePath);
  }

  public async Task<List<TagIdEntry>> GetExitZones() {
    var query = new StringBuilder();
    query.Append("Select [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]");
    query.Append("From [  ...  ]");
    query.Append("IN");
    query.Append("[  ...  ] ");
    query.Append("Where [Tag ID] = ...");
    query.Append("UNION ALL");
    query.Append("ORDER BY [Tag ID], [Reported], [Signal Strength]");

    return await ExecuteQuery<TagIdEntry>(query.ToString());

  }

  #endregion

  #region Locations

  public async Task<List<MinerEntry>> GetLocations() {

    var query = new StringBuilder();

    query.Append("Select t4.[Tag ID], t4.[MinerID], t4.[Last Name], t4.[First Name], t4.[Address], t4.[ZoneNumber], t4.[Zone], t4.[Reported], t4.[Signal Strength]");
    query.AppendLine(" From (Select [Tag ID], Max([Reported]) as Latest From [TagReader] Group By [Tag ID]) as t2 Inner Join (Select * From [TagReader] as t1");
    query.AppendLine(" Where Not Exists (Select * from [ExitZone] as t3 Where t1.[Address] = t3.[Address] And t1.[ZoneNumber] = t3.[ZoneNumber])) as t4");
    query.AppendLine(" On (t4.[Tag ID] = t2.[Tag ID]) And (t4.[Reported] = t2.[Latest]) Order By t4.[Last Name], t4.[First Name], t4.[Tag ID], t4.[Signal Strength] DESC");

    return await ExecuteQuery<MinerEntry>(query.ToString());
  }

  #endregion

  #region Alarms

  public async Task<List<AlertEntry>> GetAlerts() {

    var query = new StringBuilder();

    query.Append("Select [Address], [Device], [Type], [Alarm], [Occured], [Location], [Acknowledged], [Note]");
    query.AppendLine("FROM [Alarm]");

    return await ExecuteQuery<AlertEntry>(query.ToString());
  }

  #endregion


  #region Not Yet Implemented

  public async Task<List<MinerEntry>> GetEquipment() => await Task.Run(() => new List<MinerEntry>());
  public async Task<List<MinerEntry>> GetSupplyCars() => await Task.Run(() => new List<MinerEntry>());
  public async Task<List<MinerEntry>> GetTags() => await Task.Run(() => new List<MinerEntry>());

  #endregion
}
