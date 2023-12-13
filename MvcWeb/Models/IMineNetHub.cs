// todo:  10 Oct 2023 - JWL -- this really should be moved to Models namespace + split into seperate files.

namespace MvcWeb.Models {
  public interface IMineNetHub {
    Task AlertNotification(IAlert alertUpdate);
    Task LocationNotification(ILocationUpdate locationUpdate);
    Task TagHistoryEtlUpdate(ITagHistoryEtlUpdate tagHistoryEtlUpdate);
  }

  public interface IAlert {
    string Message { get; }
    List<AlertEntry> Alerts { get; }
  }

  public class Alert : IAlert {
    public string Message { get; set; } = string.Empty;
    public List<AlertEntry> Alerts { get; set; } = new List<AlertEntry>();

  }

  public interface ILocationUpdate {

    string Message { get; }

    List<MinerEntry> Locations { get; set; }

  }

  public class LocationUpdate : ILocationUpdate {


    public string Message { get; set; } = string.Empty;

    public List<MinerEntry> Locations { get; set; } = new List<MinerEntry>();


  }

  public interface ITagHistoryEtlUpdate {

    string Message { get; set; }

    List<TagHistoryDB> TagHistoryDBs { get; set; }

  }


  public class TagHistoryEtlUpdate : ITagHistoryEtlUpdate {

    public string Message { get; set; } = string.Empty;

    public List<TagHistoryDB> TagHistoryDBs { get; set; } = new List<TagHistoryDB>();

  }
}
