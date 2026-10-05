using EkubCircle.Application.Interfaces;
using EkubCircle.Application.Commands;
using MediatR;

namespace EkubCircle.Application.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (success, token, userId, fullName, email) = await _identityService.AuthenticateUserAsync(
            request.Email, 
            request.Password
        );

        if (!success)
        {
            throw new UnauthorizedAccessException("Invalid email or password credentials.");
        }

        return new LoginResponse(userId, fullName, email, token);
    }
}