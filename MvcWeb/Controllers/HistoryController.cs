using Microsoft.AspNetCore.Mvc;
using Serilog;

using MvcWeb.Models;

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

  #region Miners On Shift

  /* Date and Period  */
  [HttpGet]
  public async Task<IActionResult> Index() =>View(_context);

  [HttpPost]
  public async Task<IActionResult> GetMinersOnShift(DateOnly date, int period) {
    await _context.GetMinersOnShift(date, period);

    return View("Index", _context);
  }

  #endregion

  #region Summary
  public async Task<IActionResult> Summary() => View(_context); /* Id, Date and Period */

  #endregion

  #region Summary Extended
  public async Task<IActionResult> SummaryExt() => View(_context); /* Id, Date and Days */

  #endregion

  #region Snapshot
  public async Task<IActionResult> Snapshot() => View(_context);  /* Date and time */

  #endregion

}
