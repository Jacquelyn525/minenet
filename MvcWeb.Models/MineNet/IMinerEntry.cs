namespace MvcWeb.Models.MineNet;
public interface IMinerEntry {
  int TagID { get; set; }
  double MinerID { get; set; }
  string LastName { get; set; }
  string FirstName { get; set; }
  int Address { get; set; }
  int ZoneNumber { get; set; }
  string Zone { get; set; }
  DateTime Reported { get; set; }
  short SignalStrength { get; set; }
}
