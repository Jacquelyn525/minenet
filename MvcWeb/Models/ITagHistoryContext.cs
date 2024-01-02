namespace MvcWeb.Models;

public interface ITagHistoryContext {

  IEnumerable<ITagHistoryArchive> TagHistoryArchives { get; set; }
  IEnumerable<ITagIdListData> TagIdListData { get; set; }

}
