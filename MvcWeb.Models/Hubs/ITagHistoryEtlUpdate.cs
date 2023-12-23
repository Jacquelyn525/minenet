using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public interface ITagHistoryEtlUpdate {

  string Message { get; set; }

  List<ITagHistoryDB> TagHistoryDBs { get; set; }

}
