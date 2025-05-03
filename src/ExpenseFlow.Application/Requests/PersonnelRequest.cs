using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Requests;

    public class PersonnelRequest
    {
        
        public string FirstName          { get; set; } = null!;
        public string LastName           { get; set; } = null!;
        public string Email              { get; set; } = null!;
        public string PhoneNumber        { get; set; } = null!;
        public string Address            { get; set; } = null!;
        public string City               { get; set; } = null!;

    
     //   public PersonnelRole Role        { get; set; } = PersonnelRole.Personnel;
    }

