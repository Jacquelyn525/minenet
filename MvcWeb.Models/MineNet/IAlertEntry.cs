namespace MvcWeb.Models.MineNet;

public interface IAlertEntry {
  int Address { get; set; }
  string Device { get; set; }
  string Type { get; set; }
  string Alarm { get; set; }
  string Occured { get; set; }
  string Location { get; set; }
  string Acknowledged { get; set; }
  string Note { get; set; }
}
