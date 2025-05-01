// src/ExpenseFlow.Application/Validation/AccountInfoValidator.cs
using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation;

    public class AccountInfoValidator : AbstractValidator<AccountInfoRequest>
    {
        public AccountInfoValidator()
        {
            RuleFor(x => x.PersonnelId).NotEmpty();
            RuleFor(x => x.BankName).NotEmpty().Length(2,100);
            RuleFor(x => x.IBAN)
                .NotEmpty()
                .Must(i => i.StartsWith("TR") && i.Length == 26)
                .WithMessage("IBAN must be TR + 24 alphanumeric chars");
            RuleFor(x => x.AccountType).NotEmpty();
        }
    }

