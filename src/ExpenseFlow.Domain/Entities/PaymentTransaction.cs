namespace ExpenseFlow.Domain.Entities;

    public enum TransferType { EFT, Havale }
    public enum TransactionStatus { Success, Failed }

    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        
        // Hangi masrafa ait olduğu
        public Guid ExpenseId { get; set; }

        // Hangi personel hesabına EFT yapılacağı
        public Guid AccountInfoId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }

        // EFT veya Havale
        public TransferType TransferType { get; set; } = TransferType.EFT;

        // Başarılı mı, başarısız mı?
        public TransactionStatus Status { get; set; }

        // Banka entegrasyonundan dönen referans no
        public string? BankReference { get; set; }

        
        
        public Expense Expense { get; set; } = null!;
        public AccountInfo AccountInfo { get; set; } = null!;
    }

