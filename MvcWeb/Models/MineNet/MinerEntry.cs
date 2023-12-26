namespace MvcWeb.Models.MineNet {

  [ParadoxDbModel]
  public class MinerEntry {

    [ParadoxDbColumn("Tag ID")]
    public int TagID { get; set; }

    [ParadoxDbColumn("MinerID")]
    public double MinerID { get; set; }

    [ParadoxDbColumn("Last Name")]
    public string LastName { get; set; } = string.Empty;

    [ParadoxDbColumn("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [ParadoxDbColumn("Address")]
    public int Address { get; set; }

    [ParadoxDbColumn("ZoneNumber")]
    public int ZoneNumber { get; set; }

    [ParadoxDbColumn("Zone")]
    public string Zone { get; set; } = string.Empty;

    [ParadoxDbColumn("Reported")]
    public DateTime Reported { get; set; }

    [ParadoxDbColumn("Signal Strength")]
    public short SignalStrength { get; set; }

  }
}
