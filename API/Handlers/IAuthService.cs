using API.Domain;
using API.Models;

namespace API.Handlers;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    Task<AppUser?> CurrentUserAsync();
} 