using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Serilog;

using MvcWeb.Models;
using ILogger = Serilog.ILogger;

namespace MvcWeb.WS {

  // todo:  10 Oct 2023 - JWL -- this maybe should be an IHostedService on a background thread...
  [SignalRHub]
  [AllowAnonymous]
  public class MineNetHub : Hub<IMineNetHub> {

    #region CTOR & Locals

    private readonly ILogger _log = Log.Logger;
    public MineNetHub() {
      _log.Information("MineNet Hub initializing.");
    }

    #endregion

    #region Hub & Connection Lifecycle

    public override async Task OnConnectedAsync() {
      await Task.Run(() => {
        _log.Information($"New Connection: {Context.User}");
        base.OnConnectedAsync();
      });
    }

    #endregion

    #region Hub Messaging Methods

    public async Task AlertNotification(IAlert update) {
      _log.Information("Alert Notification Sent!");
      await Clients.All.AlertNotification(update);
    }

    public async Task LocationNotification(ILocationUpdate update) {
      _log.Information("LocationUpdate Notification Sent!");
      await Clients.All.LocationNotification(update);
    }

    public async Task TagHistoryEtlUpdate(ITagHistoryEtlUpdate update) {
      _log.Information("Tag History Etl Update Sent!");
      await Clients.All.TagHistoryEtlUpdate(update);
    }

    #endregion
  }
}
