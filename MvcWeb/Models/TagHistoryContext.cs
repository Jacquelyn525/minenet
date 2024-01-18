using System.Collections;
using System.Diagnostics;
using System.Linq;

using Microsoft.CodeAnalysis.Elfie.Model;

using MvcWeb.Paradox;

using Serilog;

namespace MvcWeb.Models;


[TransientService]
public class TagHistoryContext : ITagHistoryContext {

  #region CTOR and locals

  private readonly Serilog.ILogger _log = Log.Logger;
  private string historyRoot { get => $@"{_settings.MineNetConfig.MineNetPath}\{_settings.MineNetConfig.HistoryPath}"; }
  private readonly Settings _settings;
  private readonly DbAdapter _dbAdapter = null;


  public TagHistoryContext(Settings settings, DbAdapter dbAdapter) {
    _settings = settings;
    _dbAdapter = dbAdapter;

    init();
  }

  #endregion

  private void init() {

    var timer = new Stopwatch();
    timer.Start();

    //B: Run stuff you want timed

    //GetHistoryArchiveDbFiles();
    timer.Stop();
    TimeSpan timeTaken = timer.Elapsed;
    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
    _log.Debug(foo);
  }


  public IEnumerable<ITagHistoryArchive> TagHistoryArchives { get; set; } = new List<ITagHistoryArchive>();

  public IEnumerable<ITagIdListData> TagIdListData { get; set; } = new List<ITagIdListData>();

  private void GetHistoryArchiveDbFiles() {
    var tagRoot = $@"{historyRoot}\TagID";
    var paths = new List<ITagHistoryArchive>();

    if (Directory.Exists(tagRoot)) {
      var fsDirectories = Directory.EnumerateDirectories(tagRoot);

      foreach (var fsDirectory in fsDirectories) {
        var fullPaths = Directory.EnumerateFiles(fsDirectory)
                          .Where(p => p.Replace($"{fsDirectory}\\", "")
                            .Length == 11 && p.ToLower()
                            .EndsWith(".db"));

        foreach (var fullPath in fullPaths) {
          var path = new TagHistoryArchive {
            DatePath = fsDirectory.Replace($"{tagRoot}\\", ""),
            TimePath = fullPath.Replace($"{fsDirectory}\\", "").ToLower().Replace(".db", ""),
            FileNamePath = fullPath.Replace(_settings.MineNetConfig.MineNetPath, "")
          };

          //var TagIdEntries = _dbAdapter.GetArchiveUniqueTagIds(path).Result;

          //path.TagIdListData = TagIdEntries.ToList<ITagIdListData>();
          //path.TagIdListData = new List<ITagIdListData>();

          paths.Add(path);

        }
      }


      // look here for possible perf. improvements
      var tagIdListData = paths.Select(p => _dbAdapter.GetArchiveUniqueTagIds(p));

      var res = Task.WhenAll(tagIdListData).Result;

      //this.TagIdListData = results.ToList<ITagIdListData>();
      TagIdListData = MergeResults(res);

      TagHistoryArchives = paths;
    }
  }

  static List<ITagIdListData> MergeResults(List<TagIdListData>[] results) {
    // Merge the results from different databases
    var mergedResult = new List<ITagIdListData>();

    foreach (var result in results) {
      mergedResult.AddRange(result);
    }

    return mergedResult;
  }



}
