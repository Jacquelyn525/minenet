namespace MvcWeb.Models;

public interface ITagIdListData {
  int TagID { get; set; }
  string FirstName { get; set; }
  string LastName { get; set; }
  virtual string FullName {
    get {
      return $"{LastName}, {FirstName}";
    }
  }
}

[ParadoxDbModel]
public class TagIdListData : ITagIdListData {
  //public string FullName { get; set; } = string.Empty;
  //public string FullName {
  //  get {
  //    return $"{LastName}, {FirstName}";
  //  }
  //}


  [ParadoxDbColumn("Tag ID")]
  public int TagID { get; set; }


  [ParadoxDbColumn("Last Name")]
  public string LastName { get; set; } = string.Empty;

  [ParadoxDbColumn("First Name")]
  public string FirstName { get; set; } = string.Empty;

  [ParadoxDbColumn("MinerID")]
  public double MinerID { get; set; }
}

