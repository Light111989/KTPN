using System;
using API.Data;
using API.Domain;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.BienCheListing;

public class Listing
{
    public class Filter
    {
        public string FilterText { get; set; }
    }

    public class ListBienChesQuery : IRequest<BienChePaging>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }
    public class Handler : IRequestHandler<ListBienChesQuery, BienChePaging>
    {
        private readonly ApplicationDbContext _context;
        public Handler(ApplicationDbContext context) => _context = context;

        public async Task<BienChePaging> Handle(ListBienChesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.BienChes
       .Include(b => b.Khoi)
       .Include(b => b.LinhVuc)
       .AsNoTracking()
       .ToListAsync(cancellationToken);

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
                                SLHD111 = b.SLHD111,
                                KhoiId = b.KhoiId,
                                LinhVucId = b.LinhVucId
                            }).ToList()
                        }).ToList()
                }).ToList();

            var totalRecords = data.Count;

            return new BienChePaging
            {
                Items = items,
                TotalRecords = totalRecords
            };

        }
    }

}

