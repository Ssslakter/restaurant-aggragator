using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class ProfileWithRolesDTO : ProfileDTO
{
    public IEnumerable<RoleType> Roles { get; set; }

    public static ProfileWithRolesDTO FromProfileDTO(ProfileDTO profile)
    {
        return new ProfileWithRolesDTO
        {
            Id = profile.Id,
            Email = profile.Email,
            Name = profile.Name,
            Surname = profile.Surname,
            MiddleName = profile.MiddleName,
            BirthDate = profile.BirthDate,
            Gender = profile.Gender,
            Phone = profile.Phone,
            Roles = new List<RoleType>()
        };
    }
}
