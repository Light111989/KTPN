using System;
using API.Data;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.LinhVucListing;

public class Listing
{
    public class ListLinhVucsQuery : IRequest<IEnumerable<LinhVuc>>
    {
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<ListLinhVucsQuery, IEnumerable<LinhVuc>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LinhVuc>> Handle(ListLinhVucsQuery request, CancellationToken cancellationToken)
        {
            return await _context.LinhVucs.ToListAsync(cancellationToken);
        }
    }
}

