namespace MvcWeb.Models;

public interface ITagHistoryContext {

  IList<ITagHistoryArchive> TagHistoryArchives { get; set; }
  IList<ITagIdListData> TagIdListData { get; set; }

}
