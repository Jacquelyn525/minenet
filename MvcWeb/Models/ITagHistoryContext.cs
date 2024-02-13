namespace MvcWeb.Models;

public interface ITagHistoryContext {

  //Dictionary<string, ITagHistoryDb> HistoryDbDictionary { get; }

  List<ITagHistoryArchive> ArchiveDbs { get; }
  IEnumerable<ITagIdListData> TagIdListData { get; }
}
