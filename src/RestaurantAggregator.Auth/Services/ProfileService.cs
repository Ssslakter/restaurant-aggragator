using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.Data.DTO;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Services;

public interface IProfileService
{
    Task UpdateProfileAsync(ProfileCreation profileCreation);
    Task<ProfileDTO> GetProfileAsync(ClaimsPrincipal userClaims);
    Task UpdateEmailAsync(string email, string newEmail);
}

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public ProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ProfileDTO> GetProfileAsync(ClaimsPrincipal userClaims)
    {
        var user = await _userManager.GetUserAsync(userClaims);
        if (user == null)
            throw new NotFoundInDbException("User not found");
        return new ProfileDTO
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Surname = user.FullName.Split(' ')[0],
            MiddleName = user.FullName.Split(' ')[2],
            Phone = user.Phone,
            Gender = user.Gender,
            BirthDate = user.BirthDate
        };
    }

    public async Task UpdateEmailAsync(string email, string newEmail)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new NotFoundInDbException("User not found");
        if (await _userManager.FindByEmailAsync(newEmail) != null)
            throw new DbViolationException("User with this email already exists");

        user.Email = newEmail;
        user.UserName = newEmail;
        await _userManager.UpdateAsync(user);
    }

    public async Task UpdateProfileAsync(ProfileCreation profileDTO)
    {
        var user = await _userManager.FindByEmailAsync(profileDTO.Email);
        if (user == null)
            throw new NotFoundInDbException("User not found");

        user.FullName = $"{profileDTO.Surname} {profileDTO.Name} {profileDTO.MiddleName}";
        user.BirthDate = profileDTO.BirthDate;
        user.Phone = profileDTO.Phone;
        user.Name = profileDTO.Name;
        user.Gender = profileDTO.Gender;
        await _userManager.UpdateAsync(user);
    }
}
