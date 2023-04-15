namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class ProfileDTO
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public Core.Data.Enums.Gender Gender { get; set; }
    public string Phone { get; set; }
}
