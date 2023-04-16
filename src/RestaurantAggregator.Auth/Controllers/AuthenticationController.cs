using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : AuthControllerBase
{
    private readonly IUserAuthentication _authentificationService;

    public AuthenticationController(IUserAuthentication authentificationService)
    {
        _authentificationService = authentificationService;
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
    public async Task<ActionResult> Logout()
    {
        await _authentificationService.Logout(UserId);
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenModel>> RefreshToken(RefreshDTO refreshToken)
    {
        var user = await _authentificationService.ValidateRefreshToken(refreshToken.RefreshToken, refreshToken.UserId);
        return Ok(await _authentificationService.GenerateTokenPairAsync(user));
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
    {
        await _authentificationService.ChangePassword(UserId, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
        return Ok();
    }
}
