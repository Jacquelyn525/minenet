using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public interface ILocationUpdate {

  string TimeStamp { get; }

  string Message { get; }

  List<IMinerEntry> Locations { get; set; }

}
