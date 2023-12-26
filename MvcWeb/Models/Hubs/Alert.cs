using MvcWeb.Models.MineNet;

namespace MvcWeb.Models.Hubs;

public class Alert : IAlert {
  public string Message { get; set; } = string.Empty;
  public List<IAlertEntry> Alerts { get; set; } = new List<IAlertEntry>();

}

