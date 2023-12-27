namespace MvcWeb.Models.MineNet {

  [ParadoxDbModel]
  public class AlertEntry : IAlertEntry {

    [ParadoxDbColumn("Address")]
    public int Address { get; set; }

    [ParadoxDbColumn("Device")]
    public string Device { get; set; }

    [ParadoxDbColumn("Type")]
    public string Type { get; set; }

    [ParadoxDbColumn("Alarm")]
    public string Alarm { get; set; }

    [ParadoxDbColumn("Occured")]
    public string Occured { get; set; }

    [ParadoxDbColumn("Location")]
    public string Location { get; set; }

    [ParadoxDbColumn("Acknowledged")]
    public string Acknowledged { get; set; }

    [ParadoxDbColumn("Note")]
    public string Note { get; set; }
  }
}
