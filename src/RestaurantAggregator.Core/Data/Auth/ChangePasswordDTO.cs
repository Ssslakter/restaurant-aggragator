namespace RestaurantAggregator.Core.Data.Auth;
#nullable disable
public class ChangePasswordDTO
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
