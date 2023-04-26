using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.MVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Users()
    {
        //get list of all users from api
        return View();
    }
}
