using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using MvcWeb.Models.Hubs;
using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;
using MvcWeb.Services.Hubs;

using Serilog;

namespace MvcWeb.Services;

public class NotificationService : IHostedService, IDisposable {

  #region setup

  private readonly Serilog.ILogger _log = Log.Logger;
  private Timer? _timer = null;

  private readonly Settings _settings;
  private readonly IHubContext<MineNetHub, IMineNetHub> _hubContext;
  DbAdapter _dbAdapter = null;

  private readonly int MinerInterval = 30;

  public NotificationService(Settings settings, IHubContext<MineNetHub, IMineNetHub> hubContext, DbAdapter dbAdapter) {
    //public NotificationService(Settings settings, IHubContext<MineNetHub, IMineNetHub> hubContext) {
    _settings = settings;
    _hubContext = hubContext;
    MinerInterval = _settings.MineNetConfig.MinerInterval;
    _dbAdapter = dbAdapter;
  }

  #region Timer

  public async Task StartAsync(CancellationToken cancellationToken) {
    await Task.Run(() => {
      if (MinerInterval != -1) {
        _log.Information("Location Notification Service starting.");

        try {
          _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(MinerInterval));
          _log.Information("Location Notification Service started.");
        } catch (Exception ex) {
          _log.Information("Error starting Location Notification Service:");
          _log.Information(ex.ToString());
          _log.Error(ex.Message, ex);
        }
      } else {
        _log.Information($"Location Notification Service disabled in configuration: (MinerInterval: -1)");
      }
    });
  }

  public async Task StopAsync(CancellationToken cancellationToken) {
    await Task.Run(() => {
      try {
        _log.Information($"Location Notification Serivce stopping.");
        _timer?.Change(Timeout.Infinite, 0);
      } catch (Exception ex) {
        _log.Error(ex.Message);
      }
      return Task.CompletedTask;
    });
    _log.Information($"Location Notification Serivce stopped.");
  }

  public void Dispose() => _timer?.Dispose();

  private async void DoWork(object? state) {
    await LocationsReader();
    await AlertsReader();
  }

  #endregion

  #endregion

  #region Locations

  private async Task LocationsReader() {
    //var miners = await _dbAdapter.GetLocations();
    var miners = new List<IMinerEntry>();
    await sendHubUpdate(nameof(LocationsReader), miners);
  }

  private async Task sendHubUpdate(string message, List<IMinerEntry>? miners = null) {
    _log.Information(message);

    await _hubContext.Clients.All.LocationNotification(
      new LocationUpdate {
        Message = message,
        Locations = miners != null ? miners : new List<IMinerEntry>(),
      }
    );
  }

  #endregion

  #region Alerts

  private async Task AlertsReader() {
    await sendHubUpdate(nameof(LocationsReader), new List<IAlertEntry>());
  }

  private async Task sendHubUpdate(string message, List<IAlertEntry>? alerts = null) {
    _log.Information(message);

    IAlert alert = new Alert {
      Message = message,
      Alerts = alerts != null ? alerts : new List<IAlertEntry>(),
    };

    await _hubContext.Clients.All.AlertNotification(alert);
  }

  #endregion

}
