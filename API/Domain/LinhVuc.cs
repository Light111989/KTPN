using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Domain;

public class LinhVuc
{
    public Guid LinhVucId { get; set; }

    [Required, MaxLength(200)]
    public string TenLinhVuc { get; set; }

    public ICollection<Khoi> Khois { get; set; } = new List<Khoi>();
}


