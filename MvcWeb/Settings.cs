using System.Runtime.Versioning;

using Microsoft.Extensions.Configuration;

namespace MvcWeb {

  //[SupportedOSPlatform("windows")]
  public class Settings {

    public Dictionary<string, string> ConnectionStrings { get; private set; } = new Dictionary<string, string>();

    public Settings(IConfiguration configuration) {
      //ConnectionStrings[Insight.Database.ConnectionStrings.MsSqlDb] = configuration[Insight.Database.ConnectionStrings.MsSqlDb];
      //ConnectionStrings[Insight.Database.ConnectionStrings.SqLiteDb] = configuration[Insight.Database.ConnectionStrings.SqLiteDb];

      configuration.SetProps(this, nameof(ConnectionStrings));

      //if ((WebClientUrl ?? "").EndsWith("/")) {
      //  WebClientUrl = WebClientUrl[0..^1];
      //}

      //MineNetConfig = configuration.GetSectionAs<MineNetConfiguration>("MineNet");
      //JwtConfig = configuration.GetSectionAs<JwtConfiguration>("Jwt");
      //LdapConfig = configuration.GetSectionAs<LdapConfiguration>("Ldap");
    }


    //public string WebClientUrl { get; private set; } = string.Empty;
    //public LdapConfiguration LdapConfig { get; set; } = new LdapConfiguration();
    //public JwtConfiguration JwtConfig { get; set; } = new JwtConfiguration();
    //public MineNetConfiguration MineNetConfig { get; set; } = new MineNetConfiguration();


    #region Probably don't need this here...

    public Dictionary<string, int>? CacheDurations { get; private set; }

    #endregion

  }
}
