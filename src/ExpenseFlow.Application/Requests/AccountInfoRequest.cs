using System;

namespace ExpenseFlow.Application.Requests;

    public class AccountInfoRequest
    {
        public Guid PersonnelId  { get; set; }
        public string BankName   { get; set; } = null!;
        public string IBAN       { get; set; } = null!;
        public string AccountType{ get; set; } = null!;
    }

