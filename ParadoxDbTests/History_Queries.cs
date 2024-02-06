using Moq;

using MvcWeb;
using MvcWeb.Models.Configuration;
using MvcWeb.Paradox;


namespace ParadoxDbTests;

[TestClass]
public class History_Queries {

  private DbAdapter _adapter;
  private DbAdapter adapter {
    get {

      if (_adapter == null) {
        var settings = new Mock<Settings>();
        var mnConfig = new MineNetConfiguration {
          MineNetPath = @"D:\\MineNet",
          DataPath = "Data",
          HistoryPath = "History",
          MinerInterval = 1000,
          PageName = "Dev Test",
          HistoryADGroups = ["MineNet History", "Some Other History AD Group"],
        };

        settings.Setup(s => s.MineNetConfig).Returns(mnConfig);

        _adapter = new DbAdapter(settings.Object);
      }

      return _adapter;
    }
  }


  [TestMethod]
  public void Read_History_Base_Query_Returns_Expected_Data() {



  }

}

