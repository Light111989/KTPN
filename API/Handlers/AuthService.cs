using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Domain;
using API.Models;
using Microsoft.AspNetCore.Http;

namespace API.Handlers;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAccessor _userAccessor;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IUserAccessor userAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _userAccessor = userAccessor;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var token = await GenerateJwtTokenAsync(user);
            
            // Create AppUser object with user information
            var appUser = new AppUser
            {
                UserName = user.UserName ?? "",
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                AuthToken = token
            };

            return new AuthResponse
            {
                Success = true,
                Message = "User registered successfully",
                User = appUser
            };
        }

        return new AuthResponse
        {
            Success = false,
            Message = string.Join(", ", result.Errors.Select(e => e.Description))
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtTokenAsync(user);
            
            // Create AppUser object with user information
            var appUser = new AppUser
            {
                UserName = user.UserName ?? "",
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                AuthToken = token
            };

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                User = appUser
            };
        }

        return new AuthResponse
        {
            Success = false,
            Message = "Invalid email or password"
        };
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKey123!@#$%^&*()"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id),
            new Claim("emailaddress", user.Email ?? ""),
            new Claim("FirstName", user.FirstName ?? ""),
            new Claim("LastName", user.LastName ?? "")
        };

        // Add roles as a real array (multiple claims with the same type)
        foreach (var role in roles)
        {
            claims.Add(new Claim("roles", role));
        }

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AppUser?> CurrentUserAsync()
    {
        var username = _userAccessor.GetUserNameFromToken();
        if (string.IsNullOrEmpty(username))
            return null;

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return null;

        var token = await GenerateJwtTokenAsync(user);

        return new AppUser
        {
            UserName = user.UserName ?? "",
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            AuthToken = token
        };
    }
} 