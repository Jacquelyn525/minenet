using System;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Serilog;

using MvcWeb.Models;
using MvcWeb.Models.MineNet;
using MvcWeb.Paradox;



//[Route("[controller]")]
public class HistoryController : Controller {

  #region Setup

  private readonly Serilog.ILogger _log = Log.Logger;
  private readonly TagHistoryContext _context;
  private readonly IWebHostEnvironment _webHostEnvironment;

  public HistoryController(IWebHostEnvironment webHostEnvironment, TagHistoryContext context) {
    _webHostEnvironment = webHostEnvironment;
    _context = context;
  }


  #endregion

  #region Views

  //[AllowAnonymous]
  public async Task<IActionResult> Index() => View(_context); /* Date and Period  */
  public async Task<IActionResult> Summary() => View(_context); /* Id, Date and Period */
  public async Task<IActionResult> SummaryExt() => View(_context); /* Id, Date and Days */
  public async Task<IActionResult> Snapshot() => View(_context);  /* Date and time */

  #endregion


}
