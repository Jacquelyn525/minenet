using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using MvcWeb.Models.Hubs;

using Serilog;

using ILogger = Serilog.ILogger;

namespace MvcWeb.Services.Hubs;

[AllowAnonymous]
public class MineNetHub : Hub<IMineNetHub> {


  private readonly ILogger _log = Log.Logger;
  public MineNetHub() {
    _log.Information("MineNet Hub initializing.");
  }

  public override async Task OnConnectedAsync() {
    await Task.Run(() => {
      _log.Information($"New Connection: {Context.User}");
      base.OnConnectedAsync();
    });
  }

  public async Task AlertNotification(IAlert update) {
    _log.Information("Alert Notification Sent!");
    await Clients.All.AlertNotification(update);
  }

  public async Task LocationUpdate(ILocationUpdate update) {
    _log.Information("LocationUpdate Notification Sent!");
    await Clients.All.LocationNotification(update);
  }

}
