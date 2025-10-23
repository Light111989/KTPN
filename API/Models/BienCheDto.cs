
using System.Text.Json.Serialization;
using API.Domain;

public class BienCheDto
{
    public Guid Id { get; set; }
    public string TenDonVi { get; set; }
    public byte SLVienChuc { get; set; }
    public byte SLHopDong { get; set; }
    public byte SLHopDongND { get; set; }
    public byte SLBoTri { get; set; }
    public string SoQuyetDinh { get; set; }
    public byte SLGiaoVien { get; set; }
    public byte SLQuanLy { get; set; }
    public byte SLNhanVien { get; set; }
    [JsonPropertyName("slHD111")]
    public int SLHD111 { get; set; }
    public DateTime EffectiveDate { get; set; }

    public Guid KhoiId { get; set; }
    public string? TenKhoi { get; set; }
    public Guid LinhVucId { get; set; }
    public string? TenLinhVuc { get; set; }
}
