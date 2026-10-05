using FluentValidation;
using EkubCircle.Application.Commands;
namespace EkubCircle.Application.Validators;

public class ProcessRoundPayoutCommandValidator : AbstractValidator<ProcessRoundPayoutCommand>
{
    public ProcessRoundPayoutCommandValidator()
    {
        RuleFor(x => x.RoundId)
            .GreaterThan(0).WithMessage("Round ID must be greater than 0.");

        RuleFor(x => x.RequestingUserId)
            .NotEmpty().WithMessage("Requesting User ID is required.");
    }
}