using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.Auth;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : AuthControllerBase
{
    private readonly IUserAuthentication _authentificationService;
    private readonly IBanService _banService;

    public AuthenticationController(IUserAuthentication authentificationService, IBanService banService)
    {
        _authentificationService = authentificationService;
        _banService = banService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login(LoginModel loginModel)
    {
        var user = await _authentificationService.Login(loginModel.Email, loginModel.Password);
        if (await _banService.IsBannedAsync(user.Id))
            throw new ForbidException("This user is banned");
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
        if (await _banService.IsBannedAsync(user.Id))
            throw new ForbidException("This user is banned");
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
