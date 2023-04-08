using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Services;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authentificationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authentificationService, ILogger<AuthenticationController> logger)
    {
        _authentificationService = authentificationService;
        _logger = logger;
    }

    [HttpPost("login")]
    public ActionResult<string> Login(LoginModel loginModel)
    {
        var user = _authentificationService.Authenticate(loginModel.Email, loginModel.Password);
        var token = _authentificationService.GenerateJwtToken(user);
        return Ok(token);
    }

    [HttpPost("register")]
    public IActionResult Register()
    {
        return Ok();
    }
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok();
    }
}
