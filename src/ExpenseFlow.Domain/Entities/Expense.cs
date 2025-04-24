namespace ExpenseFlow.Domain.Entities;

    public enum ExpenseStatus { Pending, Approved, Rejected }

    public class Expense
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }
        public int CategoryId { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string? RejectReason { get; set; }

        public ICollection<ExpenseAttachment>? Attachments { get; set; }
        public ICollection<PaymentTransaction>? Transactions { get; set; }
    
    }

