using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using MvcWeb.Models.Hubs;
using MvcWeb.Models.MineNet;
using MvcWeb.ParadoxAdapter;
using MvcWeb.Services.Hubs;

using Serilog;

namespace MvcWeb.Services;

public class NotificationService : IHostedService, IDisposable {

  private readonly ILogger _log = Log.Logger;
  private Timer? _timer = null;
  private readonly Settings _settings;
  private readonly IHubContext<MineNetHub, IMineNetHub> _hubContext;
  private readonly int MinerInterval = 30;
  //private readonly GetLocation _getLocation;

  public NotificationService(Settings settings, IHubContext<MineNetHub, IMineNetHub> hubContext) {
    _settings = settings;
    _hubContext = hubContext;
    MinerInterval = _settings.MineNetConfig.MinerInterval;
    //_getLocation = getLocation;
  }

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
    var workerMsg = $"Location Update Successful: {DateTime.Now.ToShortTimeString()}";

    //var locations = await _getLocation.Execute();
    //var alerts = await _getAlerts.Execute();

    //await sendHubUpdate(workerMsg, locations);
    //await sendHubUpdate(workerMsg, alerts);
    await sendHubUpdate(workerMsg, new List<MinerEntry>());
  }

  private void processTags() {
    var workerMsg = $"Location Update Successful: {DateTime.Now.ToShortTimeString()}";

    //var locations = await _getLocation.Execute();

    //await sendHubUpdate(workerMsg, locations);
  }

  private void processAlerts() {
    //var workerMsg = $"Alerts Update Successful: {DateTime.Now.ToShortTimeString()}";

    //var locations = await _getLocation.Execute();

    //await sendHubUpdate(workerMsg, locations);
  }

  private async Task sendHubUpdate(string message, List<MinerEntry>? miners = null) {
    _log.Information(message);

    await _hubContext.Clients.All.LocationNotification(
      new LocationUpdate {
        Message = message,
        Locations = miners != null ? miners : new List<MinerEntry>(),
      }
    );
  }

}
