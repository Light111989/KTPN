using API.Models;

namespace API.Domain;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public AppUser? User { get; set; }
}