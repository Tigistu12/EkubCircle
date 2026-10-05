using FluentValidation;
using EkubCircle.Application.Commands;
namespace EkubCircle.Application.Validators;

public class CreateCircleCommandValidator : AbstractValidator<CreateCircleCommand>
{
    public CreateCircleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Circle name is required.")
            .MaximumLength(100).WithMessage("Circle name must not exceed 100 characters.");

        RuleFor(x => x.ContributionAmount)
            .GreaterThan(0).WithMessage("Contribution amount must be greater than 0.");

        RuleFor(x => x.MeetingLabel)
            .NotEmpty().WithMessage("Meeting label (e.g., Weekly, Monthly) is required.")
            .MaximumLength(50).WithMessage("Meeting label must not exceed 50 characters.");

        RuleFor(x => x.OrganizerUserId)
            .NotEmpty().WithMessage("Organizer user ID is required.");
    }
}