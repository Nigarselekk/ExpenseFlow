namespace ExpenseFlow.Application.Requests;

public class PaymentTransactionRequest
{
    public Guid ExpenseId { get; set; }
    public Guid AccountInfoId { get; set; }
    public decimal Amount { get; set; }
    public string? BankReference { get; set; }
    public DateTime TransactionDate { get; set; }
    public int TransferType { get; set; }
    public int Status { get; set; }
}

