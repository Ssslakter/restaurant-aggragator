using Microsoft.AspNetCore.Identity;
using RestaurantAggregator.Auth.Data.Entities;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.Services;

public interface IProfileService
{
    Task UpdateProfileAsync(Guid userId, ProfileCreation profileCreation);
    Task<ProfileDTO> GetProfileAsync(Guid userId);
}

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public ProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ProfileDTO> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        return new ProfileDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.FullName.Split(' ')[0],
            MiddleName = user.FullName.Split(' ')[2],
            Phone = user.Phone,
            Gender = user.Gender,
            BirthDate = user.BirthDate
        };
    }

    public async Task UpdateProfileAsync(Guid userId, ProfileCreation profileDTO)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        if (profileDTO.Email != user.Email)
        {
            var emailUser = await _userManager.FindByEmailAsync(profileDTO.Email);
            if (emailUser != null)
                throw new DbViolationException("User with this email already exists");
        }

        user.Email = profileDTO.Email;
        user.FullName = $"{profileDTO.Surname} {profileDTO.Name} {profileDTO.MiddleName}";
        user.BirthDate = profileDTO.BirthDate;
        user.Phone = profileDTO.Phone;
        user.Name = profileDTO.Name;
        user.Gender = profileDTO.Gender;
        await _userManager.UpdateAsync(user);
    }
}
