using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation;

    public class ExpenseValidator : AbstractValidator<ExpenseRequest>
    {
        public ExpenseValidator()
        {
            RuleFor(x => x.PersonnelId)
                .NotEmpty().WithMessage("PersonnelId is required.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(5, 500).WithMessage("Description must be between 5 and 500 characters.");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");
        }
    }

