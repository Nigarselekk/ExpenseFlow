using FluentValidation;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Validation ;

    public class PersonnelValidator : AbstractValidator<PersonnelRequest>
    {
        public PersonnelValidator()
        {   
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .Length(2, 100).WithMessage("FirstName must be between 2 and 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .Length(2, 100).WithMessage("LastName must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("PhoneNumber must be 7–15 digits.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

        }
    }

