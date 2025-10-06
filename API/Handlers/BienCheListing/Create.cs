using System;
using API.Data;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.BienCheListing
{
    public class Create
    {
        public class CreateBienCheCommand : IRequest<BienChe>
        {
            public Guid KhoiId { get; set; }
            public Guid LinhVucId { get; set; }
            public string TenDonVi { get; set; }
            public byte SLVienChuc { get; set; }
            public byte SLHopDong { get; set; }
            public byte SLHopDongND { get; set; }
            public byte SLBoTri { get; set; }
            public string SoQuyetDinh { get; set; }
            public byte SLGiaoVien { get; set; }
            public byte SLQuanLy { get; set; }
            public byte SLNhanVien { get; set; }
        }

        public class Handler : IRequestHandler<CreateBienCheCommand, BienChe>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<BienChe> Handle(CreateBienCheCommand request, CancellationToken cancellationToken)
            {
                // Kiểm tra existence của Khoi và LinhVuc
                if (!await _context.Khois.AnyAsync(k => k.KhoiId == request.KhoiId, cancellationToken))
                    throw new ArgumentException("KhoiId does not exist.");

                if (!await _context.LinhVucs.AnyAsync(lv => lv.LinhVucId == request.LinhVucId, cancellationToken))
                    throw new ArgumentException("LinhVucId does not exist.");

                // Tạo entity mới
                var bienche = new BienChe
                {
                    Id = Guid.NewGuid(),
                    KhoiId = request.KhoiId,
                    LinhVucId = request.LinhVucId,
                    TenDonVi = request.TenDonVi,
                    SLVienChuc = request.SLVienChuc,
                    SLHopDong = request.SLHopDong,
                    SLHopDongND = request.SLHopDongND,
                    SLBoTri = request.SLBoTri,
                    SoQuyetDinh = request.SoQuyetDinh,
                    SLGiaoVien = request.SLGiaoVien,
                    SLQuanLy = request.SLQuanLy,
                    SLNhanVien = request.SLNhanVien
                };

                _context.BienChes.Add(bienche);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateException ex)
                {
                    // Ghi log chi tiết
                    Console.WriteLine("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
                    throw;
                }

                return bienche;

            }
        }
    }
}