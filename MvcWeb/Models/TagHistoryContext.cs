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

  public List<ITagHistoryArchive> ArchiveDbs { get; protected set; } = new List<ITagHistoryArchive>();

  public IEnumerable<ITagIdListData> TagIdListData { get; set; } = new List<ITagIdListData>();



}

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
