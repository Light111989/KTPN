using System;

namespace API.Models;

public class BienCheFilter
{
    public Guid Id { get; set; }
    public string TenDonVi { get; set; }

    public Guid KhoiId { get; set; }
    public string TenKhoi { get; set; }

    public Guid LinhVucId { get; set; }
    public string TenLinhVuc { get; set; }
}


