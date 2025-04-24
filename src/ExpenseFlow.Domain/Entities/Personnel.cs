namespace ExpenseFlow.Domain.Entities;

    public class Personnel
    {
        public string ApplicationUserId { get; set; } = null!;
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
    

        public ICollection<AccountInfo> Accounts { get; set; } = new List<AccountInfo>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
