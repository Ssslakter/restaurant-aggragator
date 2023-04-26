using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.MVC.Models;
using LoginModelMVC = RestaurantAggregator.Core.Data.DTO.LoginModel;

namespace RestaurantAggregator.MVC.Controllers;

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

    [HttpPost]
    public ActionResult Login(LoginModelMVC model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Error");
        }

        return View("Index", model);
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
