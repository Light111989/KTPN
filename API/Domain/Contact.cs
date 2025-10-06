using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Domain;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    
    [JsonIgnore]
    public virtual Company? Company { get; set; } = null;
} 