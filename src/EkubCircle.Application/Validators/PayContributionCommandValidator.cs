using FluentValidation;
using EkubCircle.Application.Commands;

namespace EkubCircle.Application.Validators;

public class PayContributionCommandValidator : AbstractValidator<PayContributionCommand>
{
    public PayContributionCommandValidator()
    {
        RuleFor(x => x.RoundId)
            .GreaterThan(0).WithMessage("Round ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}