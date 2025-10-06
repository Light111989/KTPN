using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Companies;

public class Delete
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FindAsync(request.Id);
            
            if (company == null)
            {
                return false;
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
} 