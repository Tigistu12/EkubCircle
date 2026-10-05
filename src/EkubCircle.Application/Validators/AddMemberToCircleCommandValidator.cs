using FluentValidation;
using EkubCircle.Application.Commands;

namespace EkubCircle.Application.Validators;

public class AddMemberToCircleCommandValidator : AbstractValidator<AddMemberToCircleCommand>
{
    public AddMemberToCircleCommandValidator()
    {
        RuleFor(x => x.CircleId)
            .GreaterThan(0).WithMessage("Circle ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters.");
    }
}