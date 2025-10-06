using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Companies;

public class GetById
{
    public class GetCompanyByIdQuery : IRequest<Company?>
    {
        public Guid Id { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<GetCompanyByIdQuery, Company?>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Companies.FindAsync(request.Id);
        }
    }
} 