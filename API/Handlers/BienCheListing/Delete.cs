
using System;
using API.Data;
using MediatR;

namespace API.Handlers.BienCheListing;

public class Delete
{
    public class DeleteBiencheCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<DeleteBiencheCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteBiencheCommand res, CancellationToken cancellationToken)
        {
            var bienche = await _context.BienChes.FindAsync(res.Id);

            if (bienche == null)
            {
                return false;
            }

            _context.BienChes.Remove(bienche);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

