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

#region NOT ME

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

  private async Task<List<T>> ExecuteQuery<T>(string query, string dateFolder = null) {
    return await Task.Run(async () => {
      var builder = dateFolder == null
        ? _settings.MineNetConfig.DbConnStr()
        : _settings.MineNetConfig.HistConnStr(dateFolder);

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

  #endregion

  #endregion

  #region History

  public async Task<List<TagIdEntry>> GetMinersOnShift(string datePath, string timePath) => await ExecuteQuery<TagIdEntry>(Queries.History.Select.DailyRawFrom(timePath), datePath);

  public async Task<List<TagIdEntry>> GetMinersOnShift2(IEnumerable<ITagHistoryArchive> srcTables) {


    return new List<TagIdEntry>();
  }

  #endregion

  #region NOT ME

  #region Locations

  public async Task<List<MinerEntry>> GetTags() => await ExecuteQuery<MinerEntry>(Queries.Locations.GetTags);

  public async Task<List<MinerEntry>> GetLocations() => await ExecuteQuery<MinerEntry>(Queries.Locations.GetLocations);

  public async Task<List<MinerEntry>> GetEquipment() => await ExecuteQuery<MinerEntry>(Queries.Locations.GetEquipment);

  public async Task<List<MinerEntry>> GetSupplyCars() => await ExecuteQuery<MinerEntry>(Queries.Locations.GetSupplyCars);


  #endregion

  #region Alarms

  public async Task<List<AlertEntry>> GetAlerts() => await ExecuteQuery<AlertEntry>(Queries.Alerts.GetAlerts);

  #endregion

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
