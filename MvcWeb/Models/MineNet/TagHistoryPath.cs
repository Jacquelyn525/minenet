namespace MvcWeb.Models.MineNet {
  public class TagHistoryDB {
    public string DatePath { get; set; } = string.Empty;
    public string TimePath { get; set; } = string.Empty;
    public string FileNamePath { get; set; } = string.Empty;
    public List<TagIdEntry> TagIdEntries { get; set; } = new List<TagIdEntry>();
  }
}
