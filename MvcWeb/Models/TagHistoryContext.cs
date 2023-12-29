using System.Collections;

using MvcWeb.Paradox;

using Serilog;

namespace MvcWeb.Models;


[SingletonService]
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
    GetHistoryArchiveDbFiles();
  }


  public IList<ITagHistoryArchive> TagHistoryArchives { get; set; }

  public IList<ITagIdListData> TagIdListData { get; set; }

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

          var TagIdEntries = _dbAdapter.GetArchiveUniqueTagIds(path).Result;

          path.TagIdListData = TagIdEntries.ToList<ITagIdListData>();

          paths.Add(path);

        }
      }

      TagHistoryArchives = paths;
    }
  }

}
