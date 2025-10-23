using System;
using API.Data;
using API.Domain;
using API.Handlers.BienCheListing;
using API.Models;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class BienChesController : BaseController
{
    private readonly ApplicationDbContext _context;

    // ✅ Inject DbContext qua constructor
    public BienChesController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search(string? tenDonVi, Guid? khoiId, Guid? linhVucId, DateTime? effectiveDate)
    {
        var query = _context.BienChes
            .Include(b => b.Khoi)
            .Include(b => b.LinhVuc)
            .AsQueryable();

        if (!string.IsNullOrEmpty(tenDonVi))
            query = query.Where(b => b.TenDonVi.Contains(tenDonVi));

        if (khoiId.HasValue)
            query = query.Where(b => b.KhoiId == khoiId);

        if (linhVucId.HasValue)
            query = query.Where(b => b.LinhVucId == linhVucId);
        if (effectiveDate.HasValue)
        {
            var date = effectiveDate.Value.Date;
            query = query.Where(b => b.EffectiveDate.Date == date);
            // 👆 so sánh theo ngày (không tính giờ phút giây)
        }
        var data = await query.ToListAsync();

        var items = data
            .GroupBy(b => new { b.LinhVucId, b.LinhVuc.TenLinhVuc })
            .Select(lv => new BienCheTreeDto
            {
                LinhVucId = lv.Key.LinhVucId,
                TenLinhVuc = lv.Key.TenLinhVuc,
                Khois = lv
                    .GroupBy(b => new { b.KhoiId, b.Khoi.TenKhoi })
                    .Select(k => new KhoiNode
                    {
                        KhoiId = k.Key.KhoiId,
                        TenKhoi = k.Key.TenKhoi,
                        BienChes = k.Select(b => new BienCheDto
                        {
                            Id = b.Id,
                            TenDonVi = b.TenDonVi,
                            SLVienChuc = b.SLVienChuc,
                            SLHopDong = b.SLHopDong,
                            SLHopDongND = b.SLHopDongND,
                            SLBoTri = b.SLBoTri,
                            SoQuyetDinh = b.SoQuyetDinh,
                            SLGiaoVien = b.SLGiaoVien,
                            SLQuanLy = b.SLQuanLy,
                            SLNhanVien = b.SLNhanVien,
                            KhoiId = b.KhoiId,
                            TenKhoi = b.Khoi.TenKhoi,
                            LinhVucId = b.LinhVucId,
                            TenLinhVuc = b.LinhVuc.TenLinhVuc,
                            SLHD111 = b.SLHD111,
                            EffectiveDate = b.EffectiveDate,
                        }).ToList()
                    }).ToList()
            }).ToList();

        return Ok(new
        {
            Items = items,
            TotalRecords = data.Count
        });
    }

    [HttpGet("{id}/detail")]
    public async Task<ActionResult<object>> GetBienCheDetail(Guid id)
    {
        var bienche = await _context.BienChes
            .Where(b => b.Id == id)
            .Select(b => new BienCheDto
            {
                Id = b.Id,
                TenDonVi = b.TenDonVi,
                SLVienChuc = b.SLVienChuc,
                SLHopDong = b.SLHopDong,
                SLHopDongND = b.SLHopDongND,
                SLBoTri = b.SLBoTri,
                SoQuyetDinh = b.SoQuyetDinh,
                SLGiaoVien = b.SLGiaoVien,
                SLQuanLy = b.SLQuanLy,
                SLNhanVien = b.SLNhanVien,
                SLHD111 = b.SLHD111,
                EffectiveDate = b.EffectiveDate,
                KhoiId = b.KhoiId,
                LinhVucId = b.LinhVucId
            })
            .FirstOrDefaultAsync();

        if (bienche == null) return NotFound();

        var history = await _context.BienCheHistories
            .Where(h => h.BienCheId == id)
            .OrderByDescending(h => h.CreatedAt)
            .Select(h => new BienCheHistoryDto
            {
                Id = h.Id,
                BienCheId = h.BienCheId,
                SoQuyetDinh = h.SoQuyetDinh,
                SLVienChuc = h.SLVienChuc,
                SLHopDong = h.SLHopDong,
                SLHopDongND = h.SLHopDongND,
                SLBoTri = h.SLBoTri,
                SLGiaoVien = h.SLGiaoVien,
                SLQuanLy = h.SLQuanLy,
                SLNhanVien = h.SLNhanVien,
                SLHD111 = h.SLHD111,
                EffectiveDate = h.EffectiveDate,
                CreatedAt = h.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            current = bienche,
            history = history
        });
    }


    // GET: api/
    [HttpPost("listing")]
    public async Task<ActionResult> GetBienChes([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        var result = await Mediator.Send(new Listing.ListBienChesQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }


    // GET: api/
    [HttpGet("{id}")]
    public async Task<ActionResult<BienChe>> GetBienChe(Guid id)
    {
        var bienche = await Mediator.Send(new GetById.GetBienCheByIdQuery { Id = id });

        if (bienche == null)
        {
            return NotFound();
        }

        return Ok(bienche);
    }

    // POST: api/
    [HttpPost]
    public async Task<ActionResult<BienChe>> CreateBienChe(Create.CreateBienCheCommand command)
    {
        try
        {
            return await Mediator.Send(command);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBienChe(Guid id, [FromBody] Update.UpdateBienCheCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await Mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBienChe(Guid id)
    {
        var deleted = await Mediator.Send(new Delete.DeleteBiencheCommand { Id = id });

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }



    [HttpPost("export-excel")]
    public async Task<IActionResult> ExportExcelAsync([FromBody] ExportFilter? filter)
    {
        filter ??= new ExportFilter();
        if (filter.Type == "history")
        {
            var data = await _context.BienCheHistories
                .Include(b => b.BienChe)
                    .ThenInclude(bc => bc.Khoi)
                .Include(b => b.BienChe)
                    .ThenInclude(bc => bc.LinhVuc)
                .AsNoTracking()
                .ToListAsync();

            // filter
            if (!string.IsNullOrEmpty(filter.TenDonVi))
                data = data.Where(x => x.BienChe.TenDonVi.Contains(filter.TenDonVi)).ToList();

            if (filter.KhoiId.HasValue)
                data = data.Where(x => x.BienChe.KhoiId == filter.KhoiId.Value).ToList();

            if (filter.LinhVucId.HasValue)
                data = data.Where(x => x.BienChe.LinhVucId == filter.LinhVucId.Value).ToList();

            if (filter.FromDate.HasValue)
                data = data.Where(x => x.EffectiveDate >= filter.FromDate.Value).ToList();

            if (filter.ToDate.HasValue)
                data = data.Where(x => x.EffectiveDate <= filter.ToDate.Value).ToList();

            // Group thành tree
            var items = data
                .GroupBy(b => new { b.BienChe.LinhVucId, b.BienChe.LinhVuc.TenLinhVuc })
                .Select(lv => new
                {
                    LinhVucId = lv.Key.LinhVucId,
                    TenLinhVuc = lv.Key.TenLinhVuc,
                    Khois = lv.GroupBy(b => new { b.BienChe.KhoiId, b.BienChe.Khoi.TenKhoi })
                              .Select(k => new
                              {
                                  KhoiId = k.Key.KhoiId,
                                  TenKhoi = k.Key.TenKhoi,
                                  Histories = k.Select(h => new
                                  {
                                      h.Id,
                                      h.SoQuyetDinh,
                                      h.SLVienChuc,
                                      h.SLHopDong,
                                      h.SLHopDongND,
                                      h.SLBoTri,
                                      h.SLGiaoVien,
                                      h.SLQuanLy,
                                      h.SLNhanVien,
                                      h.SLHD111,
                                      h.EffectiveDate,
                                      h.CreatedAt,
                                      DonVi = h.BienChe.TenDonVi
                                  }).ToList()
                              }).ToList()
                }).ToList();

            // Xuất Excel
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Lịch sử biên chế");


            // --- Thiết kế header (merge cells & style) ---
            // Mình giả định bắt đầu header từ row 2 (để giống mẫu)
            // Dòng 2-4: tạo header lớn và các merge
            // Bạn có thể điều chỉnh vị trí cell nếu muốn khác

            // Set column widths (tùy chỉnh)
            ws.Column("A").Width = 4;   // STT
            ws.Column("B").Width = 40;  // Đơn vị
            ws.Column("C").Width = 12;  // Viên chức (QĐ56)
            ws.Column("D").Width = 12;  // Hợp đồng (QĐ56)
            ws.Column("E").Width = 15;  // Hợp đồng theo NĐ (QĐ7140)
            ws.Column("F").Width = 8;   // cột số ?
            ws.Column("G").Width = 42;  // Quyết định text
            ws.Column("H").Width = 12;  // Tổng cộng đầu năm
            ws.Column("I").Width = 12;  // Giáo viên
            ws.Column("J").Width = 9;   // Quản lý
            ws.Column("K").Width = 9;   // Nhân viên
            ws.Column("L").Width = 8;   // HĐ111
            ws.Column("M").Width = 12;  // Cột cuối (so sánh)
            ws.Column("N").Width = 14;  // Ngày QĐ

            // Set ROW HEIGHT (tùy chỉnh)
            ws.Row(3).Height = 40;
            // Top header area merges and labels
            // Row 2: main header rows
            ws.Range("A2:A4").Merge().Value = "ST\nT";
            ws.Range("B2:B4").Merge().Value = "ĐƠN VỊ";

            // Group: Biên chế giao theo QĐ56 (C2:D3)
            ws.Range("C2:D2").Merge().Value = "Biên chế giao theo\nQĐ 56/QĐ-UBND\nngày 02/10/2024";
            ws.Range("C3:C4").Merge().Value = "Viên chức";
            ws.Range("D3:D4").Merge().Value = "Hợp đồng";

            // Group: QĐ7140
            ws.Range("E2").Merge().Value = "QĐ 7140/QĐ-UBND\nngày 28/11/2024";
            ws.Range("E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            // Column E is "Hợp đồng theo NĐ 111..." — we place header in E3
            ws.Range("E3:E4").Merge().Value = "Hợp đồng theo NĐ\n111/2022/NĐ-CP";
            ws.Range("E3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            // Quyết định tiếp nhận ... group (F2:G3)
            // ws.Range("F2:F3").Merge().Value = "4"; // small index placeholder
            ws.Range("F2:G4").Merge().Value = "Quyết định tiếp nhận và bố trí công tác đối với viên chức sau sắp xếp";
            ws.Range("F2:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("F2:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("F2:G4").Style.Alignment.WrapText = true;

            // Số biên chế giao đầu năm + bổ sung (H2:K2)
            ws.Range("H2:K2").Merge().Value = "Số biên chế giao Đầu năm 2025+ bổ sung";
            // Subcolumns H3..K3: Tổng cộng, Giáo viên, Quản lý, Nhân viên
            ws.Range("H3:H4").Merge().Value = "Tổng cộng";
            ws.Range("I3:I4").Merge().Value = "Giáo viên";
            ws.Range("J3:J4").Merge().Value = "Quản lý";
            ws.Range("K3:K4").Merge().Value = "Nhân viên";

            // HĐ111 (L2:L4)
            ws.Range("L2:L4").Merge().Value = "HĐ 111";

            // Right-most comparison column
            ws.Range("M2:M4").Merge().Value = "Số giao đầu năm so với số hiện tại\n6 so 4";
            ws.Range("N2:N4").Merge().Value = "Ngày Quyết Định";

            // Style header region
            var headerRange = ws.Range("A2:N4");
            headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            headerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            headerRange.Style.Font.SetBold();
            headerRange.Style.Font.FontSize = 11;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Alignment.WrapText = true;

            // Row 4 used for roman I and group name row "I Lĩnh vực giáo dục" in sample.
            // We'll start content at row 5 (adjust as sample)
            int currentRow = 5;

            // Add top-level "I Lĩnh vực ..." header if you want group header row
            // We'll iterate each LinhVuc
            int sttLinhVuc = 0;
            int grandTotals_slVienChuc = 0;
            int grandTotals_slHopDong = 0;
            int grandTotals_slHopDongND = 0;
            int grandTotals_slBoTri = 0;
            int grandTotals_slGiaoVien = 0;
            int grandTotals_slQuanLy = 0;
            int grandTotals_slNhanVien = 0;
            int grandTotals_slHD111 = 0;
            foreach (var lv in items)
            {
                sttLinhVuc++;
                // Lĩnh vực row (pink background)
                ws.Cell(currentRow, 1).Value = sttLinhVuc; // STT
                ws.Cell(currentRow, 2).Value = $" {lv.TenLinhVuc}"; // tên lĩnh vực
                                                                    // style the whole row as header-of-group
                var groupRange = ws.Range(currentRow, 1, currentRow, 14);
                groupRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#fcf8f8ff");
                groupRange.Style.Font.SetBold();

                // compute khoi totals for display in header row (optional)
                // we will sum across all khois of this linhvuc
                int sumVien = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLVienChuc));
                int sumHop = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLHopDong));
                int sumHopND = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLHopDongND));
                int sumBoTri = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLBoTri));
                int sumGV = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLGiaoVien));
                int sumQL = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLQuanLy));
                int sumNV = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLNhanVien));
                int sumHD111 = lv.Khois.Sum(k => k.Histories.Sum(b => b.SLHD111));

                ws.Cell(currentRow, 3).Value = sumVien;
                ws.Cell(currentRow, 4).Value = sumHop;
                ws.Cell(currentRow, 5).Value = sumHopND;
                ws.Cell(currentRow, 6).Value = sumBoTri;
                ws.Cell(currentRow, 8).Value = sumVien; // H column "Tổng cộng" mirror (sample)
                ws.Cell(currentRow, 9).Value = sumGV;
                ws.Cell(currentRow, 10).Value = sumQL;
                ws.Cell(currentRow, 11).Value = sumNV;
                ws.Cell(currentRow, 12).Value = sumHD111;


                // add to grand totals
                grandTotals_slVienChuc += sumVien;
                grandTotals_slHopDong += sumHop;
                grandTotals_slHopDongND += sumHopND;
                grandTotals_slBoTri += sumBoTri;
                grandTotals_slGiaoVien += sumGV;
                grandTotals_slQuanLy += sumQL;
                grandTotals_slNhanVien += sumNV;
                grandTotals_slHD111 += sumHD111;

                currentRow++;

                // For each khoi within linhvuc
                foreach (var khoi in lv.Khois)
                {

                    int subVien = khoi.Histories.Sum(b => b.SLVienChuc);
                    int subHop = khoi.Histories.Sum(b => b.SLHopDong);
                    int subHopND = khoi.Histories.Sum(b => b.SLHopDongND);
                    int subBoTri = khoi.Histories.Sum(b => b.SLBoTri);
                    int subGV = khoi.Histories.Sum(b => b.SLGiaoVien);
                    int subQL = khoi.Histories.Sum(b => b.SLQuanLy);
                    int subNV = khoi.Histories.Sum(b => b.SLNhanVien);
                    int subHD111 = khoi.Histories.Sum(b => b.SLHD111);

                    // Khoi header row (bold small)
                    ws.Cell(currentRow, 2).Value = khoi.TenKhoi;
                    ws.Cell(currentRow, 2).Style.Font.SetBold();
                    ws.Cell(currentRow, 3).Value = subVien;
                    ws.Cell(currentRow, 4).Value = subHop;
                    ws.Cell(currentRow, 5).Value = subHopND;
                    ws.Cell(currentRow, 6).Value = subBoTri;
                    ws.Cell(currentRow, 8).Value = subVien;
                    ws.Cell(currentRow, 9).Value = subGV;
                    ws.Cell(currentRow, 10).Value = subQL;
                    ws.Cell(currentRow, 11).Value = subNV;
                    ws.Cell(currentRow, 12).Value = subHD111;

                    // style subtotal
                    var subRange = ws.Range(currentRow, 2, currentRow, 12);
                    subRange.Style.Font.SetBold();
                    subRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F5D7D7");
                    currentRow++;

                    // Each BienChe row
                    foreach (var bc in khoi.Histories)
                    {
                        ws.Cell(currentRow, 2).Value = bc.DonVi;
                        ws.Cell(currentRow, 3).Value = bc.SLVienChuc;
                        ws.Cell(currentRow, 4).Value = bc.SLHopDong;
                        ws.Cell(currentRow, 5).Value = bc.SLHopDongND;
                        ws.Cell(currentRow, 6).Value = bc.SLBoTri;
                        ws.Cell(currentRow, 7).Value = bc.SoQuyetDinh;
                        ws.Cell(currentRow, 8).Value = bc.SLVienChuc; // sample mapping to "Tổng cộng"
                        ws.Cell(currentRow, 9).Value = bc.SLGiaoVien;
                        ws.Cell(currentRow, 10).Value = bc.SLQuanLy;
                        ws.Cell(currentRow, 11).Value = bc.SLNhanVien;
                        ws.Cell(currentRow, 12).Value = bc.SLHD111;
                        ws.Cell(currentRow, 14).Value = bc.EffectiveDate;
                        ws.Cell(currentRow, 14).Style.DateFormat.Format = "dd/MM/yyyy";

                        // action columns or blank columns (L, M) can be left blank or filled if required
                        currentRow++;
                    }


                }
            }

            // Grand total row
            ws.Cell(currentRow + 1, 2).Value = "Tổng";
            ws.Cell(currentRow + 1, 3).Value = grandTotals_slVienChuc;
            ws.Cell(currentRow + 1, 4).Value = grandTotals_slHopDong;
            ws.Cell(currentRow + 1, 5).Value = grandTotals_slHopDongND;
            ws.Cell(currentRow + 1, 6).Value = grandTotals_slBoTri;
            ws.Cell(currentRow + 1, 8).Value = grandTotals_slVienChuc;
            ws.Cell(currentRow + 1, 9).Value = grandTotals_slGiaoVien;
            ws.Cell(currentRow + 1, 10).Value = grandTotals_slQuanLy;
            ws.Cell(currentRow + 1, 11).Value = grandTotals_slNhanVien;
            ws.Cell(currentRow + 1, 12).Value = grandTotals_slHD111;
            var gRange = ws.Range(currentRow + 1, 2, currentRow + 1, 11);
            gRange.Style.Font.SetBold();
            gRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;

            // Borders for used area
            var usedRange = ws.Range(2, 1, currentRow + 1, 14);
            usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Alignment fix for numeric columns
            ws.Range(5, 3, currentRow + 1, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // AutoFilter optional
            // ws.Range(4, 1, currentRow + 1, 13).SetAutoFilter();

            // Prepare stream to return


            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "BienCheHistory.xlsx");
        }
        else
        {
            // 1) Lấy dữ liệu (tùy theo structure DB của bạn)
            var data = await _context.BienChes
                .Include(b => b.Khoi)
                .Include(b => b.LinhVuc)
                .AsNoTracking()
                .ToListAsync();

            // Gom nhóm thành tree giống handler Listing
            var items = data
                .GroupBy(b => new { b.LinhVucId, TenLinh = b.LinhVuc.TenLinhVuc })
                .Select(lv => new
                {
                    LinhVucId = lv.Key.LinhVucId,
                    TenLinhVuc = lv.Key.TenLinh,
                    Khois = lv.GroupBy(b => new { b.KhoiId, TenKhoi = b.Khoi.TenKhoi })
                              .Select(k => new
                              {
                                  KhoiId = k.Key.KhoiId,
                                  TenKhoi = k.Key.TenKhoi,
                                  BienChes = k.Select(b => new
                                  {
                                      b.Id,
                                      b.TenDonVi,
                                      SLVienChuc = (int)b.SLVienChuc,
                                      SLHopDong = (int)b.SLHopDong,
                                      SLHopDongND = (int)b.SLHopDongND,
                                      SLBoTri = (int)b.SLBoTri,
                                      b.SoQuyetDinh,
                                      SLGiaoVien = (int)b.SLGiaoVien,
                                      SLQuanLy = (int)b.SLQuanLy,
                                      SLNhanVien = (int)b.SLNhanVien,
                                      SLHD111 = (int)b.SLHD111
                                  }).ToList()
                              }).ToList()
                }).ToList();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Biên chế");

            // --- Thiết kế header (merge cells & style) ---
            // Mình giả định bắt đầu header từ row 2 (để giống mẫu)
            // Dòng 2-4: tạo header lớn và các merge
            // Bạn có thể điều chỉnh vị trí cell nếu muốn khác

            // Set column widths (tùy chỉnh)
            ws.Column("A").Width = 4;   // STT
            ws.Column("B").Width = 40;  // Đơn vị
            ws.Column("C").Width = 12;  // Viên chức (QĐ56)
            ws.Column("D").Width = 12;  // Hợp đồng (QĐ56)
            ws.Column("E").Width = 15;  // Hợp đồng theo NĐ (QĐ7140)
            ws.Column("F").Width = 8;   // cột số ?
            ws.Column("G").Width = 42;  // Quyết định text
            ws.Column("H").Width = 12;  // Tổng cộng đầu năm
            ws.Column("I").Width = 12;  // Giáo viên
            ws.Column("J").Width = 9;   // Quản lý
            ws.Column("K").Width = 9;   // Nhân viên
            ws.Column("L").Width = 8;   // HĐ111
            ws.Column("M").Width = 12;  // Cột cuối (so sánh)

            // Set ROW HEIGHT (tùy chỉnh)
            ws.Row(3).Height = 40;
            // Top header area merges and labels
            // Row 2: main header rows
            ws.Range("A2:A4").Merge().Value = "ST\nT";
            ws.Range("B2:B4").Merge().Value = "ĐƠN VỊ";

            // Group: Biên chế giao theo QĐ56 (C2:D3)
            ws.Range("C2:D2").Merge().Value = "Biên chế giao theo\nQĐ 56/QĐ-UBND\nngày 02/10/2024";
            ws.Range("C3:C4").Merge().Value = "Viên chức";
            ws.Range("D3:D4").Merge().Value = "Hợp đồng";

            // Group: QĐ7140
            ws.Range("E2").Merge().Value = "QĐ 7140/QĐ-UBND\nngày 28/11/2024";
            ws.Range("E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            // Column E is "Hợp đồng theo NĐ 111..." — we place header in E3
            ws.Range("E3:E4").Merge().Value = "Hợp đồng theo NĐ\n111/2022/NĐ-CP";
            ws.Range("E3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            // Quyết định tiếp nhận ... group (F2:G3)
            // ws.Range("F2:F3").Merge().Value = "4"; // small index placeholder
            ws.Range("F2:G4").Merge().Value = "Quyết định tiếp nhận và bố trí công tác đối với viên chức sau sắp xếp";
            ws.Range("F2:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("F2:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("F2:G4").Style.Alignment.WrapText = true;

            // Số biên chế giao đầu năm + bổ sung (H2:K2)
            ws.Range("H2:K2").Merge().Value = "Số biên chế giao Đầu năm 2025+ bổ sung";
            // Subcolumns H3..K3: Tổng cộng, Giáo viên, Quản lý, Nhân viên
            ws.Range("H3:H4").Merge().Value = "Tổng cộng";
            ws.Range("I3:I4").Merge().Value = "Giáo viên";
            ws.Range("J3:J4").Merge().Value = "Quản lý";
            ws.Range("K3:K4").Merge().Value = "Nhân viên";

            // HĐ111 (L2:L4)
            ws.Range("L2:L4").Merge().Value = "HĐ 111";

            // Right-most comparison column
            ws.Range("M2:M4").Merge().Value = "Số giao đầu năm so với số hiện tại\n6 so 4";

            // Style header region
            var headerRange = ws.Range("A2:M4");
            headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            headerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            headerRange.Style.Font.SetBold();
            headerRange.Style.Font.FontSize = 11;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Alignment.WrapText = true;

            // Row 4 used for roman I and group name row "I Lĩnh vực giáo dục" in sample.
            // We'll start content at row 5 (adjust as sample)
            int currentRow = 5;

            // Add top-level "I Lĩnh vực ..." header if you want group header row
            // We'll iterate each LinhVuc
            int sttLinhVuc = 0;
            int grandTotals_slVienChuc = 0;
            int grandTotals_slHopDong = 0;
            int grandTotals_slHopDongND = 0;
            int grandTotals_slBoTri = 0;
            int grandTotals_slGiaoVien = 0;
            int grandTotals_slQuanLy = 0;
            int grandTotals_slNhanVien = 0;
            int grandTotals_slHD111 = 0;

            foreach (var lv in items)
            {
                sttLinhVuc++;
                // Lĩnh vực row (pink background)
                ws.Cell(currentRow, 1).Value = sttLinhVuc; // STT
                ws.Cell(currentRow, 2).Value = $" {lv.TenLinhVuc}"; // tên lĩnh vực
                                                                    // style the whole row as header-of-group
                var groupRange = ws.Range(currentRow, 1, currentRow, 14);
                groupRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#fcf8f8ff");
                groupRange.Style.Font.SetBold();

                // compute khoi totals for display in header row (optional)
                // we will sum across all khois of this linhvuc
                int sumVien = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLVienChuc));
                int sumHop = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLHopDong));
                int sumHopND = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLHopDongND));
                int sumBoTri = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLBoTri));
                int sumGV = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLGiaoVien));
                int sumQL = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLQuanLy));
                int sumNV = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLNhanVien));
                int sumHD111 = lv.Khois.Sum(k => k.BienChes.Sum(b => b.SLHD111));

                ws.Cell(currentRow, 3).Value = sumVien;
                ws.Cell(currentRow, 4).Value = sumHop;
                ws.Cell(currentRow, 5).Value = sumHopND;
                ws.Cell(currentRow, 6).Value = sumBoTri;
                ws.Cell(currentRow, 8).Value = sumVien; // H column "Tổng cộng" mirror (sample)
                ws.Cell(currentRow, 9).Value = sumGV;
                ws.Cell(currentRow, 10).Value = sumQL;
                ws.Cell(currentRow, 11).Value = sumNV;
                ws.Cell(currentRow, 12).Value = sumHD111;

                // add to grand totals
                grandTotals_slVienChuc += sumVien;
                grandTotals_slHopDong += sumHop;
                grandTotals_slHopDongND += sumHopND;
                grandTotals_slBoTri += sumBoTri;
                grandTotals_slGiaoVien += sumGV;
                grandTotals_slQuanLy += sumQL;
                grandTotals_slNhanVien += sumNV;
                grandTotals_slHD111 += sumHD111;

                currentRow++;

                // For each khoi within linhvuc
                foreach (var khoi in lv.Khois)
                {

                    int subVien = khoi.BienChes.Sum(b => b.SLVienChuc);
                    int subHop = khoi.BienChes.Sum(b => b.SLHopDong);
                    int subHopND = khoi.BienChes.Sum(b => b.SLHopDongND);
                    int subBoTri = khoi.BienChes.Sum(b => b.SLBoTri);
                    int subGV = khoi.BienChes.Sum(b => b.SLGiaoVien);
                    int subQL = khoi.BienChes.Sum(b => b.SLQuanLy);
                    int subNV = khoi.BienChes.Sum(b => b.SLNhanVien);
                    int subHD111 = khoi.BienChes.Sum(b => b.SLHD111);

                    // Khoi header row (bold small)
                    ws.Cell(currentRow, 2).Value = khoi.TenKhoi;
                    ws.Cell(currentRow, 2).Style.Font.SetBold();
                    ws.Cell(currentRow, 3).Value = subVien;
                    ws.Cell(currentRow, 4).Value = subHop;
                    ws.Cell(currentRow, 5).Value = subHopND;
                    ws.Cell(currentRow, 6).Value = subBoTri;
                    ws.Cell(currentRow, 8).Value = subVien;
                    ws.Cell(currentRow, 9).Value = subGV;
                    ws.Cell(currentRow, 10).Value = subQL;
                    ws.Cell(currentRow, 11).Value = subNV;
                    ws.Cell(currentRow, 12).Value = subHD111;

                    // style subtotal
                    var subRange = ws.Range(currentRow, 2, currentRow, 12);
                    subRange.Style.Font.SetBold();
                    subRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F5D7D7");
                    currentRow++;

                    // Each BienChe row
                    foreach (var bc in khoi.BienChes)
                    {
                        ws.Cell(currentRow, 2).Value = bc.TenDonVi;
                        ws.Cell(currentRow, 3).Value = bc.SLVienChuc;
                        ws.Cell(currentRow, 4).Value = bc.SLHopDong;
                        ws.Cell(currentRow, 5).Value = bc.SLHopDongND;
                        ws.Cell(currentRow, 6).Value = bc.SLBoTri;
                        ws.Cell(currentRow, 7).Value = bc.SoQuyetDinh;
                        ws.Cell(currentRow, 8).Value = bc.SLVienChuc; // sample mapping to "Tổng cộng"
                        ws.Cell(currentRow, 9).Value = bc.SLGiaoVien;
                        ws.Cell(currentRow, 10).Value = bc.SLQuanLy;
                        ws.Cell(currentRow, 11).Value = bc.SLNhanVien;
                        ws.Cell(currentRow, 12).Value = bc.SLHD111;

                        // action columns or blank columns (L, M) can be left blank or filled if required
                        currentRow++;
                    }

                    // // Khoi subtotal row
                    // int subVien = khoi.BienChes.Sum(b => b.SLVienChuc);
                    // int subHop = khoi.BienChes.Sum(b => b.SLHopDong);
                    // int subHopND = khoi.BienChes.Sum(b => b.SLHopDongND);
                    // int subBoTri = khoi.BienChes.Sum(b => b.SLBoTri);
                    // int subGV = khoi.BienChes.Sum(b => b.SLGiaoVien);
                    // int subQL = khoi.BienChes.Sum(b => b.SLQuanLy);
                    // int subNV = khoi.BienChes.Sum(b => b.SLNhanVien);

                    // ws.Cell(currentRow, 2).Value = "Tổng";
                    // ws.Cell(currentRow, 3).Value = subVien;
                    // ws.Cell(currentRow, 4).Value = subHop;
                    // ws.Cell(currentRow, 5).Value = subHopND;
                    // ws.Cell(currentRow, 6).Value = subBoTri;
                    // ws.Cell(currentRow, 8).Value = subVien;
                    // ws.Cell(currentRow, 9).Value = subGV;
                    // ws.Cell(currentRow, 10).Value = subQL;
                    // ws.Cell(currentRow, 11).Value = subNV;

                    // // style subtotal
                    // var subRange = ws.Range(currentRow, 2, currentRow, 11);
                    // subRange.Style.Font.SetBold();
                    // subRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#e4f197ff");

                    // currentRow++;
                }
            }

            // Grand total row
            ws.Cell(currentRow + 1, 2).Value = "Tổng";
            ws.Cell(currentRow + 1, 3).Value = grandTotals_slVienChuc;
            ws.Cell(currentRow + 1, 4).Value = grandTotals_slHopDong;
            ws.Cell(currentRow + 1, 5).Value = grandTotals_slHopDongND;
            ws.Cell(currentRow + 1, 6).Value = grandTotals_slBoTri;
            ws.Cell(currentRow + 1, 8).Value = grandTotals_slVienChuc;
            ws.Cell(currentRow + 1, 9).Value = grandTotals_slGiaoVien;
            ws.Cell(currentRow + 1, 10).Value = grandTotals_slQuanLy;
            ws.Cell(currentRow + 1, 11).Value = grandTotals_slNhanVien;
            ws.Cell(currentRow + 1, 12).Value = grandTotals_slHD111;
            var gRange = ws.Range(currentRow + 1, 2, currentRow + 1, 11);
            gRange.Style.Font.SetBold();
            gRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;

            // Borders for used area
            var usedRange = ws.Range(2, 1, currentRow + 1, 14);
            usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Alignment fix for numeric columns
            ws.Range(5, 3, currentRow + 1, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // AutoFilter optional
            // ws.Range(4, 1, currentRow + 1, 13).SetAutoFilter();

            // Prepare stream to return
            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var fileName = $"BienChe_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}

