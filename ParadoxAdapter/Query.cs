using System.Reflection.Metadata.Ecma335;
using System.Text;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MvcWeb.ParadoxAdapter;

public static class Query {

  public static string GetLocations {
    get {

      var query = new StringBuilder();
      // future maintainer:  I apologize for the following query
      // this application and codebase was a rewrite of an earlier, which is where these queries come from, as-is.
      query.Append("Select t4.[Tag ID], t4.[MinerID], t4.[Last Name], t4.[First Name], t4.[Address], t4.[ZoneNumber], t4.[Zone], t4.[Reported], t4.[Signal Strength]");
      query.Append(" From (Select [Tag ID], Max([Reported]) as Latest From [TagReader] Group By [Tag ID]) as t2 Inner Join (Select * From [TagReader] as t1");
      query.Append(" Where Not Exists (Select * from [ExitZone] as t3 Where t1.[Address] = t3.[Address] And t1.[ZoneNumber] = t3.[ZoneNumber])) as t4");
      query.Append(" On (t4.[Tag ID] = t2.[Tag ID]) And (t4.[Reported] = t2.[Latest]) Order By t4.[Last Name], t4.[First Name], t4.[Tag ID], t4.[Signal Strength] DESC");

      return query.ToString();
    }
  }

  public static string GetEquipment {
    get {

      var query = new StringBuilder();

      var select = "Select [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]";
      var from = "From [Equipment]";
      var orderby = "Order By[Last Name], [First Name], [Tag ID], [Signal Strength] DESC";

      return query.ToString();
    }
  }

  public static string GetSupplyCars {
    get {

      var query = new StringBuilder();
      var select = "Select [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]";
      var from = "From [Equipment]";
      var where = "WHERE ([Last Name] Like '%EQUIPMENT%') OR ([Last Name] Like '%EQUIPMENT%' AND [First Name] Like '(%)%')";
      var orderby = "Order By [First Name]";

      return query.ToString();
    }
  }

  public static string GetTags {
    get {

      var query = new StringBuilder();
      var select = "Select * from [TagReader]";

      return query.ToString();
    }
  }

}
