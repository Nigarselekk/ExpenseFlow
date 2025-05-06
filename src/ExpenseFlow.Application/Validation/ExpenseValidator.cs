using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation
{
    public class ExpenseValidator : AbstractValidator<ExpenseRequest>
    {
        public ExpenseValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than zero and must be a valid category.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(4).WithMessage("Description must be at least 4 characters long.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(250).WithMessage("Location cannot exceed 250 characters.")

                .Matches(@"^[\p{L}\d\s,.\-]+$")
                .WithMessage("Location can only contain letters, numbers, spaces, commas, periods, and hyphens.");
        }
    }
}