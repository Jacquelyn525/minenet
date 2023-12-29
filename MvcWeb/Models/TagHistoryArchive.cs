namespace MvcWeb.Models;
public class TagHistoryArchive : ITagHistoryArchive {
  public string DatePath { get; set; } = string.Empty;
  public string TimePath { get; set; } = string.Empty;
  public string FileNamePath { get; set; } = string.Empty;

  public IList<ITagIdListData> TagIdListData { get; set; } = new List<ITagIdListData>();
}
