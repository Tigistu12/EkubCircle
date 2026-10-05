using EkubCircle.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EkubCircle.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityService(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<(bool Success, string[] Errors)> RegisterUserAsync(string fullName, string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return (false, new[] { "A user with this email address already exists." });
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        return (true, Array.Empty<string>());
    }

    public async Task<(bool Success, string Token, string UserId, string FullName, string Email)> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (false, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
        {
            return (false, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, user.FullName);
        return (true, token, user.Id, user.FullName, user.Email!);
    }
}