using FluentValidation;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Validation;

    public class PaymentTransactionValidator : AbstractValidator<PaymentTransactionRequest>
    {
        public PaymentTransactionValidator()
        {
            RuleFor(x => x.ExpenseId)
                .NotEmpty().WithMessage("ExpenseId is required.");

            RuleFor(x => x.AccountInfoId)
                .NotEmpty().WithMessage("AccountInfoId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        }
    }

