using System.ComponentModel.DataAnnotations;

namespace RestaurantAggregator.Auth.Data.DTO;
#nullable disable
public class RegistrationModel
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    public string Address { get; set; }
}
