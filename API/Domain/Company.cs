using System.ComponentModel.DataAnnotations;

namespace API.Domain;

public class Company
{
    public Guid Id { get; set; }
    
    [Required]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string NameCode { get; set; } = string.Empty;
    
    public string TaxCode { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string DeliveryAddress { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Website { get; set; } = string.Empty;
    
    public string BankInfo { get; set; } = string.Empty;
    
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public bool IsVirtual { get; set; } = false;
} 