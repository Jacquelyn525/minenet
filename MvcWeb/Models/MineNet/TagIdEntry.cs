namespace MvcWeb.Models.MineNet;

[ParadoxDbModel]
public class TagIdEntry {

  [ParadoxDbColumn("Tag ID")]
  public int TagID { get; set; }

  [ParadoxDbColumn("Address")]
  public int Address { get; set; }

  [ParadoxDbColumn("ZoneNumber")]
  public int ZoneNumber { get; set; }

  [ParadoxDbColumn("Last Name")]
  public string LastName { get; set; } = string.Empty;

  [ParadoxDbColumn("First Name")]
  public string FirstName { get; set; } = string.Empty;

  [ParadoxDbColumn("Zone")]
  public string Zone { get; set; } = string.Empty;

  [ParadoxDbColumn("Reported")]
  public DateTime Reported { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

  [ParadoxDbColumn("Signal Strength")]
  public int SignalStrength { get; set; }

  [ParadoxDbColumn("MinerID")]
  public double MinerID { get; set; }
}

#region Old Version

/*
  //[ParadoxDbModel]
  //public class TagIdEntry {

  //  [ParadoxDbColumn("Tag ID")]
  //  public int TagID { get; set; }

  //  [ParadoxDbColumn("Address")]
  //  public int Address { get; set; }

  //  [ParadoxDbColumn("ZoneNumber")]
  //  public int ZoneNumber { get; set; }

  //  [ParadoxDbColumn("DateKey")]
  //  public DateTime DateKey { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

  //  [ParadoxDbColumn("MinuteKey")]
  //  public int MinuteKey { get; set; }

  //  [ParadoxDbColumn("Last Name")]
  //  public string LastName { get; set; } = string.Empty;

  //  [ParadoxDbColumn("First Name")]
  //  public string FirstName { get; set; } = string.Empty;

  //  [ParadoxDbColumn("Zone")]
  //  public string Zone { get; set; } = string.Empty;

  //  [ParadoxDbColumn("Reported")]
  //  public DateTime Reported { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

  //  [ParadoxDbColumn("Battery")]
  //  public int Battery { get; set; }

  //  [ParadoxDbColumn("Signal Strength")]
  //  public int SignalStrength { get; set; }

  //  [ParadoxDbColumn("Message")]
  //  public int Message { get; set; }

  //  [ParadoxDbColumn("Temperature")]
  //  public int Temperature { get; set; }

  //  [ParadoxDbColumn("Source")]
  //  public int Source { get; set; }

  //  [ParadoxDbColumn("Last Zone")]
  //  public string LastZone { get; set; } = string.Empty;

  //  [ParadoxDbColumn("Last Reported")]
  //  public DateTime LastReported { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

  //  [ParadoxDbColumn("Rate Reported")]
  //  public int RateReported { get; set; }

  //  [ParadoxDbColumn("Last Rate")]
  //  public int LastRate { get; set; }

  //  [ParadoxDbColumn("Message Count")]
  //  public int MessageCount { get; set; }

  //  [ParadoxDbColumn("Message Alarm")]
  //  public int MessageAlarm { get; set; }

  //  [ParadoxDbColumn("MinerID")]
  //  public double MinerID { get; set; }
  //}
*/

#endregion
