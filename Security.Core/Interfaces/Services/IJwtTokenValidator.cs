 
using System.Security.Claims;

namespace Axity.Security.Core.Interfaces.Services
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
