using System;
using API.Data;
using API.Domain;
using API.Handlers.BienCheListing;
using API.Models;
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
    public async Task<IActionResult> Search(string? tenDonVi, Guid? khoiId, Guid? linhVucId)
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
                            TenLinhVuc = b.LinhVuc.TenLinhVuc
                        }).ToList()
                    }).ToList()
            }).ToList();

        return Ok(new
        {
            Items = items,
            TotalRecords = data.Count
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
}

