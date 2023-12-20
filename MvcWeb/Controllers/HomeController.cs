using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MvcWeb.Models;

namespace MvcWeb.Controllers;

[AllowAnonymous]
public class HomeController : Controller {

  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger) {
    _logger = logger;
  }

  [AllowAnonymous]
  public IActionResult Index() {
    return View();
  }

  [AllowAnonymous]
  public IActionResult Privacy() {
    return View();
  }

  [AllowAnonymous]
  public IActionResult History() {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
