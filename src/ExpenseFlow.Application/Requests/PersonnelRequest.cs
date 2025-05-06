using System.Text.Json.Serialization;


namespace ExpenseFlow.Application.Requests;

public class PersonnelRequest
{

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Password { get; set; } = null!;


    [JsonIgnore]
    public Guid PersonnelId { get; set; }

}

