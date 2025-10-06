using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class AppUser
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string AuthToken { get; set; }
} 