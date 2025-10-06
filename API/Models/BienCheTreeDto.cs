using System;

namespace API.Models
{
    public class BienCheTreeDto
    {
        public Guid LinhVucId { get; set; }
        public string TenLinhVuc { get; set; }
        public List<KhoiNode> Khois { get; set; } = new();
    }

    public class KhoiNode
    {
        public Guid KhoiId { get; set; }
        public string TenKhoi { get; set; }
        public List<BienCheDto> BienChes { get; set; } = new();
    }
}
