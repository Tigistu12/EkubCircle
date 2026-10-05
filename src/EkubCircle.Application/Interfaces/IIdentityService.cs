namespace EkubCircle.Application.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string[] Errors)> RegisterUserAsync(string fullName, string email, string password);
    Task<(bool Success, string Token, string UserId, string FullName, string Email)> AuthenticateUserAsync(string email, string password);
}