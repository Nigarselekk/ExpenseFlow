
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Responses;

    public class PaymentTransactionResponse
    {
        public Guid Id { get; set; }
        public Guid ExpenseId { get; set; }
        public Guid AccountInfoId { get; set; }
        
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }
        public TransactionStatus Status { get; set; }
        public string? BankReference { get; set; }
    }

