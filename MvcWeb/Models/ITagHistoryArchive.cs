namespace MvcWeb.Models;

public interface ITagHistoryArchive {
  string DatePath { get; set; }
  string TimePath { get; set; }
  string FileNamePath { get; set; }
  IList<ITagIdListData> TagIdListData { get; set; }
}
