using Microsoft.AspNetCore.SignalR;

using MvcWeb.Models.Hubs;
using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;

using MvcWeb.Services.Hubs;

using Serilog;

namespace MvcWeb.Services;

public class EtlService : IHostedService {

  #region setup

  private readonly Serilog.ILogger _log = Log.Logger;
  private Timer? _timer = null;

  private readonly Settings _settings;
  private readonly IHubContext<MineNetHub, IMineNetHub> _hubContext;
  DbAdapter _dbAdapter = null;

  private readonly int EtlInterval = 30;

  public EtlService(Settings settings, IHubContext<MineNetHub, IMineNetHub> hubContext, DbAdapter dbAdapter) {
    _settings = settings;
    _hubContext = hubContext;
    EtlInterval = _settings.MineNetConfig.EtlInterval;
    _dbAdapter = dbAdapter;
  }

  #region Timer

  public async Task StartAsync(CancellationToken cancellationToken) {
    await Task.Run(() => {
      if (EtlInterval != -1) {
        _log.Information("History ETL Service starting.");

        try {
          _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(EtlInterval));
          _log.Information("History ETL Service started.");
        } catch (Exception ex) {
          _log.Information("Error starting History ETL Service:");
          _log.Information(ex.ToString());
          _log.Error(ex.Message, ex);
        }
      } else {
        _log.Information($"History ETL Service disabled in configuration: (EtlInterval: -1)");
      }
    });
  }

  public async Task StopAsync(CancellationToken cancellationToken) {
    await Task.Run(() => {
      try {
        _log.Information($"History ETL Serivce stopping.");
        _timer?.Change(Timeout.Infinite, 0);
      } catch (Exception ex) {
        _log.Error(ex.Message);
      }
      return Task.CompletedTask;
    });
    _log.Information($"History ETL Serivce stopped.");
  }

  public void Dispose() => _timer?.Dispose();

  #endregion

  #endregion
  private async void DoWork(object? state) {
    //await LocationsReader();
    //await AlertsReader();
  }


  //1. Read history directory for sub directories
  private void ReadDirectories() => throw new NotImplementedException();


  ////2. iterate through sub directories
  //private void ReadDirectories() => throw new NotImplementedException();
  ////3. read data from each and insert to sql db
  //private void ReadDirectories() => throw new NotImplementedException();
  //4. zip and move (or possibly delete) sub directory + files to 'read' location.
  //private void ReadDirectories() => throw new NotImplementedException();
  ////5. move next
  //private void ReadDirectories() => throw new NotImplementedException();


}
