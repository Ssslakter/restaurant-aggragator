using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Services;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserAuthentication _authentificationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUserAuthentication authentificationService, ILogger<AuthenticationController> logger)
    {
        _authentificationService = authentificationService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login(LoginModel loginModel)
    {
        var user = await _authentificationService.Login(loginModel.Email, loginModel.Password);
        return Ok(await _authentificationService.GenerateTokenPairAsync(user));
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenModel>> Register(RegistrationModel registrationModel)
    {
        var user = await _authentificationService.Register(registrationModel);
        return Ok(await _authentificationService.GenerateTokenPairAsync(user));
    }
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        await _authentificationService.Logout(userId);
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenModel>> RefreshToken([FromBody] RefreshDTO refreshToken)
    {
        var user = await _authentificationService.FindByRefreshToken(refreshToken.RefreshToken);
        return Ok(await _authentificationService.GenerateTokenPairAsync(user));
    }
}
