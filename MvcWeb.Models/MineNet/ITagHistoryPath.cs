namespace MvcWeb.Models.MineNet {
  public interface ITagHistoryDB {
    string DatePath { get; set; }
    string TimePath { get; set; }
    string FileNamePath { get; set; }
    IEnumerable<ITagIdEntry> TagIdEntries { get; set; }
  }
}
