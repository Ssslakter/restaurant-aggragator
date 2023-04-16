using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Services;
using RestaurantAggregator.Infra.Utils;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : AuthControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProfileCreation>> GetProfile()
    {
        return Ok(await _profileService.GetProfileAsync(User));
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateProfile(ProfileCreation newProfile)
    {
        await _profileService.UpdateProfileAsync(newProfile);
        return Ok();
    }

    [HttpPatch("email")]
    [Authorize]
    public async Task<ActionResult> UpdateEmail(EmailDTO newEmail)
    {
        var userEmail = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        await _profileService.UpdateEmailAsync(userEmail, newEmail.Email);
        return Ok();
    }
}
