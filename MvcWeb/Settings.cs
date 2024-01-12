using Microsoft.Extensions.Configuration;

using MvcWeb.Models.Configuration;

namespace MvcWeb {
  
  public class Settings {

    public Dictionary<string, string> ConnectionStrings { get; private set; } = new Dictionary<string, string>();

    public List<string> AllowedGroups { get; private set; } = new List<string>();

    public Settings(IConfiguration configuration) {

      configuration.SetProps(this, nameof(ConnectionStrings));

      MineNetConfig = configuration.GetSectionAs<MineNetConfiguration>("MineNet");
    }

    public MineNetConfiguration MineNetConfig { get; set; } = new MineNetConfiguration();
  }
}
