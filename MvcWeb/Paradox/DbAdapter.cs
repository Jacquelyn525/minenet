using System.Collections.Generic;
using System.Data.OleDb;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

using MvcWeb.Models;
using MvcWeb.Models.Configuration;
using MvcWeb.Models.MineNet;

using Serilog;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

  private async Task<List<MinerEntry>> ParadoxQuery(StringBuilder query) {
    return await ExecuteQuery<MinerEntry>(query.ToString());
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
    query.Append("SELECT [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]");
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

  public async Task<List<MinerEntry>> GetTags() {
    //var query = "SELECT * from [TagReader]";
    var query = new StringBuilder();

    query.Append("SELECT [Tag ID], [Address], [ZoneNumber], [DateKey], [MinuteKey], [Last Name], [First Name],");
    query.AppendLine("[Zone], [Reported], [Battery], [Signal Strength], [Message], [Temperature], [Source],");
    query.AppendLine("[Last Zone], [Last Reported], [Rate Reported], [Last Rate], [Message], [Count], [Message],");
    query.AppendLine("[Alarm], [MinerID] FROM [TagReader]");


    return await ExecuteQuery<MinerEntry>(query.ToString());
  }

  public async Task<List<MinerEntry>> GetLocations() {

    var query = new StringBuilder();

    query.AppendLine("SELECT");
    query.AppendLine("    t4.[Tag ID],");
    query.AppendLine("    t4.[MinerID],");
    query.AppendLine("    t4.[Last Name],");
    query.AppendLine("    t4.[First Name], t4.[Address],");
    query.AppendLine("    t4.[ZoneNumber], t4.[Zone],");
    query.AppendLine("    t4.[Reported],");
    query.AppendLine("    t4.[Signal Strength]");
    query.AppendLine("FROM");
    query.AppendLine("  (  ");
    query.AppendLine("    SELECT");
    query.AppendLine("        [Tag ID],");
    query.AppendLine("        Max([Reported]) as Latest");
    query.AppendLine("    From [TagReader]");
    query.AppendLine("    Group By [Tag ID]");
    query.AppendLine("  ) as t2");
    query.AppendLine("Inner JOIN");
    query.AppendLine("  (   ");
    query.AppendLine("    SELECT *");
    query.AppendLine("    From [TagReader] as t1");
    query.AppendLine("    Where Not Exists");
    query.AppendLine("      (   ");
    query.AppendLine("        SELECT *");
    query.AppendLine("        from [ExitZone] as t3");
    query.AppendLine("        Where t1.[Address] = t3.[Address] And t1.[ZoneNumber] = t3.[ZoneNumber]");
    query.AppendLine("      )   ");
    query.AppendLine("  ) as t4  ");
    query.AppendLine("  On (t4.[Tag ID] = t2.[Tag ID]) And (t4.[Reported] = t2.[Latest])");
    query.AppendLine("Order By t4.[Last Name], t4.[First Name], t4.[Tag ID], t4.[Signal Strength] DESC");

    return await ParadoxQuery(query);
  }

  public async Task<List<MinerEntry>> GetEquipment() {
    var query = new StringBuilder();

    query.Append("SELECT [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]");
    query.AppendLine("From [Equipment]");
    query.AppendLine(" Order By [Last Name], [First Name], [Tag ID], [Signal Strength] DESC");

    return await ParadoxQuery(query);
  }

  public async Task<List<MinerEntry>> GetSupplyCars() {
    var query = new StringBuilder();

    query.AppendLine("Select");
    query.AppendLine("      [Tag ID]");
    query.AppendLine("    , [MinerID]");
    query.AppendLine("    , [Last Name]");
    query.AppendLine("    , [First Name]");
    query.AppendLine("    , [Address]");
    query.AppendLine("    , [ZoneNumber]");
    query.AppendLine("    , [Zone]");
    query.AppendLine("    , [Reported]");
    query.AppendLine("    , [Signal Strength]");
    query.AppendLine("FROM");
    query.AppendLine("    [Equipment]");
    query.AppendLine("WHERE");
    query.AppendLine("      ([Last Name] Like '%EQUIPMENT%')  ");
    query.AppendLine("  OR  ");
    query.AppendLine("      ([Last Name] Like '%EQUIPMENT%' AND [First Name] Like '(%)%') ");
    query.AppendLine("Order By [First Name]");

    return await ParadoxQuery(query);
  }

  #endregion

  #region Alarms

  public async Task<List<AlertEntry>> GetAlerts() {

    var query = new StringBuilder();

    query.Append("SELECT [Address], [Device], [Type], [Alarm], [Occured], [Location], [Acknowledged], [Note]");
    query.AppendLine("FROM [Alarm]");

    return await ExecuteQuery<AlertEntry>(query.ToString());
  }

  #endregion


  #region Not Yet Implemented

  //  xmlHistory.asp
  //
  //  cWorkFlow.OnShift
  //    CDate(source),
  //    CInt(start),
  //    CInt(end)
  // 
  //  cBusLogic.GetDailyRawData()
  public async Task OnShift(DateOnly date, int periodStart, int periodEnd) {
    var query = new StringBuilder();
    query.AppendLine("          Select                        ");
    query.AppendLine("                [Tag ID]                ");
    query.AppendLine("              , [MinerID]               ");
    query.AppendLine("              , [Last Name]             ");
    query.AppendLine("              , [First Name]            ");
    query.AppendLine("              , [Address]               ");
    query.AppendLine("              , [ZoneNumber]            ");
    query.AppendLine("              , [Zone]                  ");
    query.AppendLine("              , [Reported]              ");
    query.AppendLine("              , [Signal Strength]       ");
    query.AppendLine("          From                          ");
    query.AppendLine("                [   ...  ]              ");
    query.AppendLine("                                        ");
    query.AppendLine("          IN                            ");
    query.AppendLine("                                        ");
    query.AppendLine("                [   ...  ]              ");
    query.AppendLine("                                        ");
    query.AppendLine("          Where                         ");
    query.AppendLine("                [Tag ID] = [  ...  ]    ");
    query.AppendLine("                                        ");
    query.AppendLine("          UNION ALL                     ");
    query.AppendLine("                                        ");
    query.AppendLine("          ORDER BY                      ");
    query.AppendLine("                [Tag ID]                ");
    query.AppendLine("              , [Reported]              ");
    query.AppendLine("              , [Signal Strength]       ");
  }

  /*
    https://github.com/Muqluk/minenet/blob/develop/docs/discovery/code-autopsy-notes.md

      internally referenced     cWorkFlow.GetExitZones()                                               cBusLogic.GetDailyRawData()
      xmlidHistory.asp          cWorkFlow.Location(CInt(id), CDate(source), CInt(start), CInt(end))    cBusLogic.GetDailyRawData()
      xmlidHistoryEx.asp        cWorkFlow.LocationEx(CDate(source), CInt(id), CInt(days))              cBusLogic.GetDailyRawData()
      xmlSnap.asp               cWorkFlow.Snapshot(CDate(source) + CDate(snap))                        cBusLogic.GetDailyRawData()


    */

  #endregion
}
