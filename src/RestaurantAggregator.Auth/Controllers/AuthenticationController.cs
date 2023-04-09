using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Data.Enums;
using RestaurantAggregator.Auth.Services;
using RestaurantAggregator.Auth.Utils;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserAuthentication _authentificationService;
    private readonly IRolesHandler _rolesHandler;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUserAuthentication authentificationService, IRolesHandler rolesHandler, ILogger<AuthenticationController> logger)
    {
        _authentificationService = authentificationService;
        _rolesHandler = rolesHandler;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginModel loginModel)
    {
        var user = await _authentificationService.Login(loginModel.Email, loginModel.Password);
        var token = _authentificationService.GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<ActionResult<object>> RegisterClient(RegistrationModel registrationModel)
    {
        var user = await _authentificationService.Register(registrationModel);
        try
        {
            await _rolesHandler.AddRoleToUserAsync(user, Role.Client);
        }
        catch (NotFoundInDbException e)
        {
            _logger.LogError(e, "Role Client was not found, which is not expected");
            throw new Exception();
        }
        var token = _authentificationService.GenerateJwtToken(user);
        return Ok(new { token });
    }
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Split(" ").Last();
        var userId = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        await _authentificationService.Logout(token, userId);
        return Ok();
    }
}
