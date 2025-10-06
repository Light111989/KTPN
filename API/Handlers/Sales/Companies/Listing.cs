using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Companies;

public class Listing
{
    public class ListCompaniesQuery : IRequest<IEnumerable<Company>>
    {
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<ListCompaniesQuery, IEnumerable<Company>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> Handle(ListCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Companies.ToListAsync(cancellationToken);
        }
    }
} 