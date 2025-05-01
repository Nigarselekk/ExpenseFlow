using FluentValidation;
using ExpenseFlow.Application.Requests;

namespace ExpenseFlow.Application.Validation;

    public class ExpenseAttachmentValidator : AbstractValidator<ExpenseAttachmentRequest>
    {
        public ExpenseAttachmentValidator()
        {
            RuleFor(x => x.ExpenseId)
                .NotEmpty().WithMessage("ExpenseId is required.");

            RuleFor(x => x.FileUrl)
                .NotEmpty().WithMessage("FileUrl is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("FileUrl must be a valid URL.");

            RuleFor(x => x.FileType)
                .NotEmpty().WithMessage("FileType is required.")
                .MaximumLength(50).WithMessage("FileType cannot exceed 50 characters.");
        }
    }

