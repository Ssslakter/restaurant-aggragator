using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.Auth;

namespace RestaurantAggregator.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserAuthentication _authentificationService;

    public HomeController(ILogger<HomeController> logger, IUserAuthentication authentificationService)
    {
        _logger = logger;
        _authentificationService = authentificationService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("login")]
    public ActionResult Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return Forbidden();
        }

        return View("Index", model);
    }

    [HttpGet("login")]
    public ActionResult Login()
    {
        return View();
    }

    [HttpGet("forbidden")]
    public ActionResult Forbidden()
    {
        return View();
    }

    [HttpGet("error")]
    public ActionResult Error(string message)
    {
        ViewData["ErrorMessage"] = message;
        return View();
    }
}
