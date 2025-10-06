using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Domain;

public class Khoi
{
    public Guid KhoiId { get; set; }

    [Required, MaxLength(200)]
    public string TenKhoi { get; set; }

    public Guid LinhVucId { get; set; }
    public LinhVuc LinhVuc { get; set; }   // FK đến LĩnhVực

    public ICollection<BienChe> BienChes { get; set; } = new List<BienChe>();
}


