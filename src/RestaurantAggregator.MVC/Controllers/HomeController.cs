using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Core.Data.Auth;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Infra.Auth;
using RestaurantAggregator.Infra.Config;

namespace RestaurantAggregator.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserAuthentication _authentificationService;
    private readonly IOptions<CookieConfiguration> _cookieConfiguration;
    private readonly SignInManager<User> _signInManager;
    public HomeController(
        ILogger<HomeController> logger,
        IUserAuthentication authentificationService,
        IOptions<CookieConfiguration> cookieConfiguration,
        SignInManager<User> signInManager
        )
    {
        _logger = logger;
        _authentificationService = authentificationService;
        _cookieConfiguration = cookieConfiguration;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
            return LocalRedirect("/admin/restaurant");
        return LocalRedirect("/login");
    }

    //По каким-то причинам при авторизации в админку, если пользователь не админ,
    // то MVC кидает на /Account/AccessDenied, игнорируя конфиги
    [Route("/Account/AccessDenied")]
    public IActionResult FixMVCringe()
    {
        return LocalRedirect("/forbidden");
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return LocalRedirect("/login");
        }
        var user = await _authentificationService.Login(model.Email, model.Password);

        var authProps = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.Now + _cookieConfiguration.Value.Lifetime
        };

        await _signInManager.SignInWithClaimsAsync(
            user,
            authProps,
            await _authentificationService.GetClaims(user)
        );
        return LocalRedirect("/admin/restaurant");
    }

    [RoleAuthorize(RoleType.Admin)]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return LocalRedirect("/login");
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
