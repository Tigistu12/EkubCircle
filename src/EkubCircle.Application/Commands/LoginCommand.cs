using MediatR;

namespace EkubCircle.Application.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;

public record LoginResponse(
    string UserId,
    string FullName,
    string Email,
    string Token
);