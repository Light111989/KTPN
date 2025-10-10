using System;
using System.ComponentModel.DataAnnotations;

namespace API.Domain
{
    public class BienChe
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string TenDonVi { get; set; }

        public byte SLVienChuc { get; set; }
        public byte SLHopDong { get; set; }
        public byte SLHopDongND { get; set; }
        public byte SLBoTri { get; set; }
        public string SoQuyetDinh { get; set; }
        public byte SLGiaoVien { get; set; }
        public byte SLQuanLy { get; set; }
        public byte SLNhanVien { get; set; }
        public byte SLHD111 { get; set; }

        // Foreign keys
        public Guid KhoiId { get; set; }
        public Khoi Khoi { get; set; }

        public Guid LinhVucId { get; set; }
        public LinhVuc LinhVuc { get; set; }
    }
}
