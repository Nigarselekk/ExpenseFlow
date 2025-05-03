using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Responses;

    public class PersonnelResponse
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;

    // public Expense expenses { get; set; } = null!;

     //   public PersonnelRole Role { get; set; }
    }

