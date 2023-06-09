using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.DAL.Data.Entities;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Auth.BL.Services;

public class ProfileService : IProfileService
{
    private readonly int _pageSize = 10;
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
            Email = user.Email,
            Name = user.Name,
            Surname = user.FullName?.Split(' ')[0],
            MiddleName = user.FullName?.Split(' ')[2],
            Phone = user.Phone,
            Gender = user.Gender,
            BirthDate = user.BirthDate
        };
    }

    public async Task UpdateProfileAsync(Guid userId, ProfileCreation profileCreation)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        if (profileCreation.Email != user.Email)
        {
            var emailUser = await _userManager.FindByEmailAsync(profileCreation.Email);
            if (emailUser != null)
                throw new DbViolationException("User with this email already exists");
        }

        user.Email = profileCreation.Email;
        user.FullName = $"{profileCreation.Surname} {profileCreation.Name} {profileCreation.MiddleName}";
        user.BirthDate = profileCreation.BirthDate;
        user.Phone = profileCreation.Phone;
        user.Name = profileCreation.Name;
        user.Gender = profileCreation.Gender;
        await _userManager.UpdateAsync(user);
    }

    public async Task<IEnumerable<ProfileDTO>> GetUserProfilesAsync(uint page)
    {
        var users = await _userManager.Users
        .Skip((int.Max(1, (int)page) - 1) * _pageSize).Take(_pageSize).ToListAsync();
        return users.FilterNonAdmins(_userManager).ConvertAll(user => new ProfileDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Surname = user.FullName?.Split(' ')[0],
            MiddleName = user.FullName?.Split(' ')[2],
            Phone = user.Phone,
            Gender = user.Gender,
            BirthDate = user.BirthDate
        });
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundInDbException("User not found");
        await _userManager.DeleteAsync(user);
    }
}
internal static class UsersListExtensions
{
    internal static List<User> FilterNonAdmins(this List<User> users, UserManager<User> userManager)
    {
        return users.Where(user =>
        {
            var roles = userManager.GetRolesAsync(user).Result.Select(Enum.Parse<RoleType>);
            return !roles.Contains(RoleType.Admin);
        }
         ).ToList();
    }
}
