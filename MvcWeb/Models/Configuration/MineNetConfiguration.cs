using System.Data.OleDb;
using System.Runtime.Versioning;

namespace MvcWeb.Models.Configuration {
  public interface IMineNetConfiguration {
    string MineNetPath { get; }
    string DataPath { get; }
    string HistoryPath { get; }
    string PageName { get; }
    int MinerInterval { get; }
    IEnumerable<string> HistoryADGroups { get; }
  }


  public class MineNetConfiguration : IMineNetConfiguration {
    public string MineNetPath { get; set; } = string.Empty;
    public string DataPath { get; set; } = "Data";
    public string HistoryPath { get; set; } = "History";
    public string PageName { get; set; } = string.Empty;
    public int MinerInterval { get; set; } = 30;
    public IEnumerable<string> HistoryADGroups { get; set; } = new List<string>();
  }


  #region QoL / Utility Extensions


  public static class MineNetConfigurationExtensions {
    public static OleDbConnectionStringBuilder DbConnStr(this MineNetConfiguration cfg) {
      return new OleDbConnectionStringBuilder {
        { "Provider", "Microsoft.Jet.OLEDB.4.0" },
        { "Data Source", @$"{cfg.MineNetPath}\{cfg.DataPath}" },
        { "Extended Properties", "Paradox 4.x" }
      };
    }

    public static OleDbConnectionStringBuilder HistConnStr(this MineNetConfiguration cfg, string subPath, string filename) {
      return new OleDbConnectionStringBuilder {
        { "Provider", "Microsoft.Jet.OLEDB.4.0" },
        { "Data Source", @$"{cfg.MineNetPath}\{cfg.HistoryPath}\TagID\{subPath}" },
        { "Extended Properties", "Paradox 5.x" },
      };
    }
  }

  #endregion
}
