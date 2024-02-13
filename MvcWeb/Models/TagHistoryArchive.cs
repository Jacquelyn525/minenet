namespace MvcWeb.Models;

public interface ITagHistoryArchive {
  string DatePath { get; set; }
  string TimePath { get; set; }
  string FileNamePath { get; set; }
  public DateOnly Date { get; }
  public TimeOnly Time { get; }
}

public class TagHistoryArchive : ITagHistoryArchive {
  public string DatePath { get; set; } = string.Empty;
  public string TimePath { get; set; } = string.Empty;
  public string FileNamePath { get; set; } = string.Empty;
  public TimeOnly Time { get => TimeOnly.ParseExact(TimePath, "HH mm ss"); }
  public DateOnly Date { get => DateOnly.ParseExact(DatePath, "yyyy MM dd"); }
}
