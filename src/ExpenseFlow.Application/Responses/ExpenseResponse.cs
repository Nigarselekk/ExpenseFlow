 
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Responses;

    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }
        public string PersonnelName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        
        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public string? Country { get; set; }
        public DateTime Date { get; set; }
        
        public ExpenseStatus Status { get; set; }
        public string? RejectReason { get; set; }
        
        public List<ExpenseAttachmentResponse>? Attachments { get; set; }
        public List<PaymentTransactionResponse>? Transactions { get; set; }
    }

