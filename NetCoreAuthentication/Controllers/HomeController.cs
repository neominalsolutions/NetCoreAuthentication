using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreAuthentication.Models;
using System.Diagnostics;

namespace NetCoreAuthentication.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    /// <summary>
    /// Login olan herkes girebilir
    /// </summary>
    /// <returns></returns>

    [Authorize] // aspect oriented programing, cross cutting, hata işleme exception, veri doğrulama validation, authentication authrization, logging
    public IActionResult AuthenticatedUserPage()
    {
      return View();
    }

    /// <summary>
    /// Sadece Admin Rolüne sahip olanlar girebilir
    /// </summary>
    /// <returns></returns>
    /// 
    [Authorize(Roles ="Admin")]
    public IActionResult AdminPage()
    {
      return View();
    }

    /// <summary>
    /// Sadece Manager Rolüne Sahip olanlar girebilir
    /// </summary>
    /// <returns></returns>
    /// 
    [Authorize(Roles ="Manager")]
    public IActionResult ManagerPage()
    {
      return View();
    }

    /// <summary>
    /// Sadece User Delete Permission sahip olanlar girebilir.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = "UserDeletePolicy")] // özel bir kural tanımı yapılmalı. Poliçe bazlı kurallar uygulanırken bu poliçeler uygulamanın program dosyasında oluşturulur ve buradaki isimler ile çağırılır.
    public IActionResult UserDeletePage()
    {
      return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}