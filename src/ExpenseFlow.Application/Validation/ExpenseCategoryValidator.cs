using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation;

    public class ExpenseCategoryValidator : AbstractValidator<ExpenseCategoryRequest>
    {
        public ExpenseCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(2, 100).WithMessage("Category name must be between 2 and 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

