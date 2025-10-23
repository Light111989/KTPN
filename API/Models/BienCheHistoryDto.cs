using System.Text.Json.Serialization;

public class BienCheHistoryDto
{
    public Guid Id { get; set; }
    public Guid BienCheId { get; set; }
    public string SoQuyetDinh { get; set; }
    public int SLVienChuc { get; set; }
    public int SLHopDong { get; set; }
    public int SLHopDongND { get; set; }
    public int SLBoTri { get; set; }
    public int SLGiaoVien { get; set; }
    public int SLQuanLy { get; set; }
    public int SLNhanVien { get; set; }
    [JsonPropertyName("slHD111")]
    public int SLHD111 { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime CreatedAt { get; set; }
}