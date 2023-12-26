namespace MvcWeb.Models.MineNet;

public interface ITagIdEntry {
  int TagID { get; set; }
  int Address { get; set; }
  int ZoneNumber { get; set; }
  DateTime DateKey { get; set; }
  string LastName { get; set; }
  string FirstName { get; set; }
  string Zone { get; set; }
  DateTime Reported { get; set; }
  int Battery { get; set; }
  int SignalStrength { get; set; }
  int Message { get; set; }
  int Temperature { get; set; }
  int Source { get; set; }
  string LastZone { get; set; }
  DateTime LastReported { get; set; }
  int RateReported { get; set; }
  int LastRate { get; set; }
  int MessageCount { get; set; }
  int MessageAlarm { get; set; }
  double MinerID { get; set; }
}
