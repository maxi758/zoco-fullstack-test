using System.Security.Claims;

namespace zoco.API.Common;

public static class AuthorizationExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole("Admin");
    }

    public static bool CanAccess(this ClaimsPrincipal user, Guid ownerUserId)
    {
        if (IsAdmin(user))
            return true;

        var userId = user.GetUserId();
        return userId == ownerUserId;
    }
}