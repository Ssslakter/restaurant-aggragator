using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Infra.Auth;

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
    public async Task<ActionResult<ProfileDTO>> GetProfile()
    {
        return Ok(await _profileService.GetProfileAsync(UserId));
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateProfile(ProfileCreation newProfile)
    {
        await _profileService.UpdateProfileAsync(UserId, newProfile);
        return Ok();
    }
}
