using EkubCircle.Application.Interfaces;
using MediatR;
using EkubCircle.Application.Commands;

namespace EkubCircle.Application.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (success, errors) = await _identityService.RegisterUserAsync(
            request.FullName, 
            request.Email, 
            request.Password
        );

        if (!success)
        {
            throw new InvalidOperationException(string.Join("; ", errors));
        }

        return new RegisterResponse(true, "User registered successfully.");
    }
}