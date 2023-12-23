using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public class TagHistoryEtlUpdate : ITagHistoryEtlUpdate {

  public string Message { get; set; } = string.Empty;

  public List<ITagHistoryDB> TagHistoryDBs { get; set; } = new List<ITagHistoryDB>();

}

