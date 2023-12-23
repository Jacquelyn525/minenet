using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public class LocationUpdate : ILocationUpdate {

  public string TimeStamp { get => DateTime.Now.ToLongTimeString(); }

  public string Message { get; set; } = string.Empty;

  public List<IMinerEntry> Locations { get; set; } = new List<IMinerEntry>();

}

