using Microsoft.AspNetCore.Http;

namespace API.Handlers;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserNameFromToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        // Try to get username from different possible claim types
        var username = httpContext.User.FindFirst("emailaddress")?.Value ?? 
                      httpContext.User.FindFirst("sub")?.Value ??
                      httpContext.User.FindFirst("name")?.Value ??
                      httpContext.User.Identity?.Name;

        return username;
    }
} 