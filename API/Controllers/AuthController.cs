using Microsoft.AspNetCore.Mvc;
using API.Domain;
using API.Handlers;
using Microsoft.AspNetCore.Authorization;
using API.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.RegisterAsync(request);
        
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.LoginAsync(request);
        
        if (response.Success)
        {
            return response;
        }

        return response;
    }

    [HttpGet("current-user")]
    [Authorize]
    public async Task<ActionResult<AppUser>> CurrentUser()
    {
        var user = await _authService.CurrentUserAsync();
        
        if (user == null)
        {
            return Unauthorized(new { message = "User not found or not authenticated" });
        }

        return Ok(user);
    }
} 