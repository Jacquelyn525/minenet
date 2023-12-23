using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public interface IAlert {
  string Message { get; }
  List<IAlertEntry> Alerts { get; }
}
