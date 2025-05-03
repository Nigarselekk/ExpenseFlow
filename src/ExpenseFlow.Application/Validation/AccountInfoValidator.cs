// src/ExpenseFlow.Application/Validation/AccountInfoValidator.cs
using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation;

public class AccountInfoValidator : AbstractValidator<AccountInfoRequest>
{
    public AccountInfoValidator()
    {
        RuleFor(x => x.PersonnelId).NotEmpty();
        RuleFor(x => x.BankName).NotEmpty().Length(2, 100);
        RuleFor(x => x.AccountType).NotEmpty().Length(2, 50);

        RuleFor(x => x.IBAN)
        .NotEmpty()
        .Matches(@"^TR\d{24}$")
        .WithMessage("Please enter a valid IBAN (eg: TR012345678901234567890123).");

        RuleFor(x => x.AccountType).NotEmpty();
    }
}

