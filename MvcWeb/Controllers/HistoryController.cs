using System;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Serilog;

using MvcWeb.Models;
using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;



namespace GadgetStore.UI.MVC.Controllers;


//[Route("[controller]")]
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
  //public async Task<IActionResult> Index() {
  //  return View(_context);
  //}

  #endregion

  public async Task<IActionResult> Index(/* Date and Period  */) {

    return View(_context);
  }
  public async Task<IActionResult> Summary(/* Id, Date and Period */) {
    return View(_context);
  }
  public async Task<IActionResult> SummaryExt(/* Id, Date and Days */) {
    return View(_context);
  }
  public async Task<IActionResult> Snapshot(/* Date and time) */) {
    return View(_context);
  }

}
