using EkubCircle.Application.DTOs.Auth;
using EkubCircle.Application.DTOs.Circle;
using EkubCircle.Application.DTOs.Member;

namespace EkubCircle.Application.Interfaces;

public interface ICircleService
{
    Task<object> CreateCircleAsync(CreateCircleDto request);

    Task<object> GetCircleAsync(Guid circleId);

    Task AddMemberAsync(
        Guid circleId,
        AddMemberDto request,
        string organizerId);

    Task<List<object>> GetMembersAsync(Guid circleId);
}