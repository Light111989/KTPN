using System;
using API.Data;
using MediatR;

namespace API.Handlers.BienCheListing;

public class Update
{
    public class UpdateBienCheCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid? LinhVucId { get; set; }   // ✅ thêm dòng này
        public Guid KhoiId { get; set; }
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
    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<UpdateBienCheCommand>
    {
        private readonly ApplicationDbContext _context;
        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateBienCheCommand res, CancellationToken cancellationToken)
        {
            var bienche = await _context.BienChes.FindAsync(res.Id);

            if (bienche == null)
            {
                throw new InvalidOperationException($"List with ID {res.Id} not found.");
            }

            bienche.LinhVucId = res.LinhVucId ?? throw new ArgumentException("LinhVucId is required.");
            bienche.TenDonVi = res.TenDonVi;
            bienche.SLVienChuc = res.SLVienChuc;
            bienche.SLHopDong = res.SLHopDong;
            bienche.SLHopDongND = res.SLHopDongND;
            bienche.SLBoTri = res.SLBoTri;
            bienche.SoQuyetDinh = res.SoQuyetDinh;
            bienche.SLGiaoVien = res.SLGiaoVien;
            bienche.SLQuanLy = res.SLQuanLy;
            bienche.SLNhanVien = res.SLNhanVien;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
