using MediatR;

namespace EkubCircle.Application.Commands;

public record RegisterCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<RegisterResponse>;

public record RegisterResponse(
    bool Success,
    string Message
);