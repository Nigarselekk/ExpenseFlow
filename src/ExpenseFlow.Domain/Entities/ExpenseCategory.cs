namespace ExpenseFlow.Domain.Entities;

    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<Expense>? Expenses { get; set; }
    }
