using FluentValidation;
using EkubCircle.Application.Commands;
namespace EkubCircle.Application.Validators;

public class StartCircleCommandValidator : AbstractValidator<StartCircleCommand>
{
    public StartCircleCommandValidator()
    {
        RuleFor(x => x.CircleId)
            .GreaterThan(0).WithMessage("Circle ID must be greater than 0.");

        RuleFor(x => x.RequestingUserId)
            .NotEmpty().WithMessage("Requesting User ID is required.");
    }
}