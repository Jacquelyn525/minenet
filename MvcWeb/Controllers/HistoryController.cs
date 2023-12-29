using System;

using Microsoft.AspNetCore.Mvc;

using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;


namespace GadgetStore.UI.MVC.Controllers;


//[Route("[controller]")]
public class HistoryController : Controller {

  #region Setup

  private readonly MvcWeb.Paradox.DbAdapter _dbAdapter;
  private readonly IWebHostEnvironment _webHostEnvironment;


  public HistoryController(IWebHostEnvironment webHostEnvironment, DbAdapter dbAdapter) {
    _webHostEnvironment = webHostEnvironment;
    _dbAdapter = dbAdapter;
  }

  //[AllowAnonymous]
  public async Task<IActionResult> Index() {
    return View();
  }

  #endregion

  public async Task<IActionResult> MinersOnShift(/* Date and Period  */) {
    //return View(await _dbAdapter.GetExitZones());
    //var list = new List<TagIdEntry>();

    return View();
  }
  public async Task<IActionResult> MinerSummary(/* Id, Date and Period */) {
    return View(new List<TagIdEntry>());
  }
  public async Task<IActionResult> MinerSummaryExtended(/* Id, Date and Days */) {
    return View(new List<TagIdEntry>());
  }
  public async Task<IActionResult> MineSnapshot(/* Date and time) */) {
    return View(new List<TagIdEntry>());
  }

}
