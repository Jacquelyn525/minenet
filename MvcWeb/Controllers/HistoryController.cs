using System;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Serilog;

using MvcWeb.Models;
using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;
using Microsoft.AspNetCore.Authorization;



namespace GadgetStore.UI.MVC.Controllers;


//[Route("[controller]")]
[Authorize(Policy = "RequireWindowsGroup")]
public class HistoryController : Controller {

  #region Setup

  private readonly Serilog.ILogger _log = Log.Logger;
  private readonly ITagHistoryContext _context;
  private readonly IWebHostEnvironment _webHostEnvironment;


  public HistoryController(IWebHostEnvironment webHostEnvironment, TagHistoryContext context) {
    _webHostEnvironment = webHostEnvironment;
    _context = context;
  }

  //[AllowAnonymous]
  public async Task<IActionResult> Index() {
    return View(_context);
  }

  #endregion

  public async Task<IActionResult> MinersOnShift(/* Date and Period  */) {
    //return View(await _dbAdapter.GetExitZones());
    //var list = new List<TagIdEntry>();
    //_log.Debug(JsonConvert.SerializeObject(_context.TagHistoryArchives));

    return View(_context);
  }
  public async Task<IActionResult> MinerSummary(/* Id, Date and Period */) {
    return View(_context);
  }
  public async Task<IActionResult> MinerSummaryExtended(/* Id, Date and Days */) {
    return View(_context);
  }
  public async Task<IActionResult> MineSnapshot(/* Date and time) */) {
    return View(_context);
  }

}
