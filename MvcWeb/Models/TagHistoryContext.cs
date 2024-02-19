using Serilog;
using MvcWeb.Paradox;
using MvcWeb.Models.MineNet;

namespace MvcWeb.Models;


[TransientService]
public class TagHistoryContext {
  #region CTOR and locals

  private readonly Serilog.ILogger _log = Log.Logger;

  private readonly Settings _settings;
  private readonly DbAdapter _dbAdapter;

  public TagHistoryContext(Settings settings, DbAdapter dbAdapter) {
    _settings = settings;
    _dbAdapter = dbAdapter;
    ArchiveDbs = IOService.collectDbArchives(settings);
  }

  #endregion

  #region Properties

  public DateOnly MinDate { get => ArchiveDbs.Min(archive => archive.Date); }
  public DateOnly MaxDate { get => ArchiveDbs.Max(archive => archive.Date); }

  public List<ITagHistoryArchive> ArchiveDbs { get; protected set; } = new List<ITagHistoryArchive>();

  public List<TagIdEntry> TagIdListData { get; set; } = new List<TagIdEntry>();

  #endregion


  public async Task GetMinersOnShift(DateOnly date, int period) {
    TagIdListData.Clear();

    DateTime start, stop;

    if (period == 0) {
      start = combine(date, TimeOnly.ParseExact("00 00 00", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("08 00 00", "HH mm ss"));
    } else if (period == 1) {
      start = combine(date, TimeOnly.ParseExact("08 00 00", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("16 00 00", "HH mm ss"));
    } else {
      start = combine(date, TimeOnly.ParseExact("16 00 00", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("23 59 59", "HH mm ss"));
    }

    var readTasks = ArchiveDbs
      .Where(archive => archive.DateTime >= start && archive.DateTime <= stop)
      .Select(archive => _dbAdapter.GetMinersOnShift(archive.DatePath, archive.TimePath));

    var results = await Task.WhenAll(readTasks);

    var mergedResults = MergeResults(results);

    var tmp = mergedResults.GroupBy(r => r.MinerID)
      .Select(m => new TagIdEntry {

      })
      .ToList();


    TagIdListData = MergeResults(results)
      .OrderBy(t => t.LastName)
      .ToList();
    var a = 1;
    //await GetMinersOnShift___NEW(date, period);
  }


  public async Task GetMinersOnShift___NEW(DateOnly date, int period) {
    DateTime start, stop;

    if (period == 0) {
      start = combine(date, TimeOnly.ParseExact("00 00 00", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("08 00 00", "HH mm ss"));
    } else if (period == 1) {
      start = combine(date, TimeOnly.ParseExact("08 00 01", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("16 00 00", "HH mm ss"));
    } else {
      start = combine(date, TimeOnly.ParseExact("16 00 01", "HH mm ss"));
      stop = combine(date, TimeOnly.ParseExact("23 59 59", "HH mm ss"));
    }

    var srcTables = ArchiveDbs.Where(archive => archive.DateTime >= start && archive.DateTime <= stop);

    var temp = _dbAdapter.GetMinersOnShift2(srcTables);

    //var results = await Task.WhenAll(readTasks);

    //TagIdListData = MergeResults(results).OrderBy(t => t.LastName).ToList();
    TagIdListData = new List<TagIdEntry>();
  }

  #region Utilities

  private static DateTime combine(DateOnly date, TimeOnly time) {
    return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
  }

  private static List<TagIdEntry> MergeResults(List<TagIdEntry>[]? results) {
    /*
    // Merge the results from different databases
    //List<TagIdEntry> mergedResult = new List<TagIdEntry>();

    //foreach (var result in results) {
    //  mergedResult.AddRange(result);
    //}
    */

    List<TagIdEntry> mergedResult = results.SelectMany(x => x).Distinct().ToList();

    return mergedResult;
  }

  #endregion
}

#region Helpers

public static class IOService {

  public static List<ITagHistoryArchive> collectDbArchives(Settings settings) {
    var cfg = settings.MineNetConfig;
    var tagRoot = $@"{cfg.MineNetPath}\{cfg.HistoryPath}\TagID";
    var ArchiveDbs = new List<ITagHistoryArchive>();

    if (Directory.Exists(tagRoot)) {
      var fsDirectories = Directory.EnumerateDirectories(tagRoot);

      foreach (var fsDirectory in fsDirectories) {
        var archiveDbPaths = DirectoryExtensions.ArchiveDbPaths(fsDirectory);

        foreach (var dbPath in archiveDbPaths) {
          var path = new TagHistoryArchive {
            DatePath = fsDirectory.Replace($"{tagRoot}\\", ""),
            TimePath = dbPath.Replace($"{fsDirectory}\\", "").ToLower().Replace(".db", ""),
            FileNamePath = dbPath.Replace(settings.MineNetConfig.MineNetPath, "")
          };

          ArchiveDbs.Add(path);
        }
      }
    }
    return ArchiveDbs;
  }

}


#endregion
