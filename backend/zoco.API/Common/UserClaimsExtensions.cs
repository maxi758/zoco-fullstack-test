using System.Security.Claims;

namespace zoco.API.Common;

public static class UserClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return value == null ? throw new UnauthorizedAccessException("UserId no encontrado en token") : Guid.Parse(value);
    }
}
