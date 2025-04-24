namespace ExpenseFlow.Domain.Entities;

    public enum TransferType { EFT, Havale }
    public enum TransactionStatus { Success, Failed }

    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid ExpenseId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }  = TransferType.EFT;
        public TransactionStatus Status { get; set; }

        public Expense Expense { get; set; } = null!;
    }

