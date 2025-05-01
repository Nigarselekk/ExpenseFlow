 

namespace ExpenseFlow.Application.Requests;

    public class PaymentTransactionRequest
    {
        public Guid ExpenseId { get; set; }
        public Guid AccountInfoId { get; set; }
        public decimal Amount { get; set; }
        

    }

