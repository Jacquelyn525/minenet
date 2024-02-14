using System.Text;

public static class Queries {

  public static class Alerts {

    public static string GetAlerts {
      get {
        var query = new StringBuilder();

        query.Append("SELECT [Address], [Device], [Type], [Alarm], [Occured], [Location], [Acknowledged], [Note]");
        query.AppendLine("FROM [Alarm]");

        return query.ToString();
      }
    }

  }

  public static class Locations {

    public static string GetTags {
      get {
        var query = new StringBuilder();

        query.Append("SELECT [Tag ID], [Address], [ZoneNumber], [DateKey], [MinuteKey], [Last Name], [First Name],");
        query.AppendLine("[Zone], [Reported], [Battery], [Signal Strength], [Message], [Temperature], [Source],");
        query.AppendLine("[Last Zone], [Last Reported], [Rate Reported], [Last Rate], [Message], [Count], [Message],");
        query.AppendLine("[Alarm], [MinerID] FROM [TagReader]");

        return query.ToString();
      }
    }

    public static string GetLocations {
      get {
        var query = new StringBuilder();

        query.AppendLine("SELECT");
        query.AppendLine("    t4.[Tag ID],");
        query.AppendLine("    t4.[MinerID],");
        query.AppendLine("    t4.[Last Name],");
        query.AppendLine("    t4.[First Name], t4.[Address],");
        query.AppendLine("    t4.[ZoneNumber], t4.[Zone],");
        query.AppendLine("    t4.[Reported],");
        query.AppendLine("    t4.[Signal Strength]");
        query.AppendLine("FROM");
        query.AppendLine("  (  ");
        query.AppendLine("    SELECT");
        query.AppendLine("        [Tag ID],");
        query.AppendLine("        Max([Reported]) as Latest");
        query.AppendLine("    From [TagReader]");
        query.AppendLine("    Group By [Tag ID]");
        query.AppendLine("  ) as t2");
        query.AppendLine("Inner JOIN");
        query.AppendLine("  (   ");
        query.AppendLine("    SELECT *");
        query.AppendLine("    From [TagReader] as t1");
        query.AppendLine("    Where Not Exists");
        query.AppendLine("      (   ");
        query.AppendLine("        SELECT *");
        query.AppendLine("        from [ExitZone] as t3");
        query.AppendLine("        Where t1.[Address] = t3.[Address] And t1.[ZoneNumber] = t3.[ZoneNumber]");
        query.AppendLine("      )   ");
        query.AppendLine("  ) as t4  ");
        query.AppendLine("  On (t4.[Tag ID] = t2.[Tag ID]) And (t4.[Reported] = t2.[Latest])");
        query.AppendLine("Order By t4.[Last Name], t4.[First Name], t4.[Tag ID], t4.[Signal Strength] DESC");

        return query.ToString();
      }
    }

    public static string GetEquipment {
      get {
        var query = new StringBuilder();

        query.Append("SELECT [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]");
        query.AppendLine("From [Equipment]");
        query.AppendLine(" Order By [Last Name], [First Name], [Tag ID], [Signal Strength] DESC");

        return query.ToString();
      }
    }

    public static string GetSupplyCars {
      get {
        var query = new StringBuilder();

        query.AppendLine("Select");
        query.AppendLine("      [Tag ID]");
        query.AppendLine("    , [MinerID]");
        query.AppendLine("    , [Last Name]");
        query.AppendLine("    , [First Name]");
        query.AppendLine("    , [Address]");
        query.AppendLine("    , [ZoneNumber]");
        query.AppendLine("    , [Zone]");
        query.AppendLine("    , [Reported]");
        query.AppendLine("    , [Signal Strength]");
        query.AppendLine("FROM");
        query.AppendLine("    [Equipment]");
        query.AppendLine("WHERE");
        query.AppendLine("      ([Last Name] Like '%EQUIPMENT%')  ");
        query.AppendLine("  OR  ");
        query.AppendLine("      ([Last Name] Like '%EQUIPMENT%' AND [First Name] Like '(%)%') ");
        query.AppendLine("Order By [First Name]");

        return query.ToString();
      }
    }

  }

  public static class History {


    public static class Select {

      public static string DailyRaw {
        get {
          var sb = new StringBuilder();
          sb.Append("SELECT");
          sb.AppendLine("   [Tag ID],");
          sb.AppendLine("   [Address],");
          sb.AppendLine("   [ZoneNumber],");
          sb.AppendLine("   [DateKey],");
          sb.AppendLine("   [MinuteKey],");
          sb.AppendLine("   [Last Name],");
          sb.AppendLine("   [First Name],");
          sb.AppendLine("   [Zone],");
          sb.AppendLine("   [Reported],");
          sb.AppendLine("   [Battery],");
          sb.AppendLine("   [Signal Strength],");
          sb.AppendLine("   [Message],");
          sb.AppendLine("   [Temperature],");
          sb.AppendLine("   [Source],");
          sb.AppendLine("   [Last Zone],");
          sb.AppendLine("   [Last Reported],");
          sb.AppendLine("   [Rate Reported],");
          sb.AppendLine("   [Last Rate],");
          sb.AppendLine("   [Message Count],");
          sb.AppendLine("   [Message Alarm],");
          sb.AppendLine("   [MinerID]");
          sb.AppendLine("");


          return sb.ToString();
        }
      }

      public static string DailyRawFrom(string timePath) {
        var query = new StringBuilder(DailyRaw);
        query.AppendLine($"FROM [{timePath}]");

        return query.ToString();
      }


      //public async Task<List<TagIdListData>> GetArchiveUniqueTagIds(ITagHistoryArchive path) {
      //  var sb = new StringBuilder();
      //  sb.Append("SELECT DISTINCT");
      //  sb.AppendLine("[Tag ID], [First Name], [Last Name], [MinerID]");
      //  sb.AppendLine($"FROM [{path.TimePath}]");
      //  sb.AppendLine("ORDER BY [Tag ID] ASC");

      //  return await ExecuteQuery<TagIdListData>(sb.ToString(), path.DatePath, path.TimePath);
      //}

      //public async Task<List<TagIdEntry>> GetExitZones() {
      //  var query = new StringBuilder();
      //  query.Append("SELECT [Tag ID], [MinerID], [Last Name], [First Name], [Address], [ZoneNumber], [Zone], [Reported], [Signal Strength]");
      //  query.Append("From [  ...  ]");
      //  query.Append("IN");
      //  query.Append("[  ...  ] ");
      //  query.Append("Where [Tag ID] = ...");
      //  query.Append("UNION ALL");
      //  query.Append("ORDER BY [Tag ID], [Reported], [Signal Strength]");

      //  return await ExecuteQuery<TagIdEntry>(query.ToString());

      //}

    }


  }
}
