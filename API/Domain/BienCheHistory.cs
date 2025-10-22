using System;
using System.ComponentModel.DataAnnotations;

namespace API.Domain;

public class BienCheHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid BienCheId { get; set; }   // FK sang BienChe
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
    public DateTime EffectiveDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Quan hệ
    public BienChe BienChe { get; set; }
}

