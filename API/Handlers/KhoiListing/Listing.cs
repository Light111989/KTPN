using System;
using API.Data;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.KhoiListing;

public class Listing
{
    public class ListKhoisQuery : IRequest<IEnumerable<Khoi>>
    {
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<ListKhoisQuery, IEnumerable<Khoi>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Khoi>> Handle(ListKhoisQuery request, CancellationToken cancellationToken)
        {
            return await _context.Khois.ToListAsync(cancellationToken);
        }
    }
}

