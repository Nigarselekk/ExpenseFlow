

namespace ExpenseFlow.Application.Responses;

    public class AccountInfoResponse
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }
        public string BankName { get; set; } = null!;
        public string IBAN { get; set; } = null!;
        public string AccountType { get; set; } = null!;
    }

