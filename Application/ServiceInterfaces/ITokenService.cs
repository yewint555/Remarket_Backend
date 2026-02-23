using System.Security.Claims;
using Domain.Entities;

namespace Application.ServiceInterfaces;

public interface ITokenService
{
    string GenerateAccessToken(Users users);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}