namespace EkubCircle.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, string fullName);
}