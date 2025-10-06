using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Companies;

public class Update
{
    public class UpdateCompanyCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameCode { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string BankInfo { get; set; } = string.Empty;
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FindAsync(request.Id);
            
            if (company == null)
            {
                throw new InvalidOperationException($"Company with ID {request.Id} not found.");
            }

            company.Code = request.Code;
            company.Name = request.Name;
            company.NameCode = request.NameCode;
            company.TaxCode = request.TaxCode;
            company.Address = request.Address;
            company.DeliveryAddress = request.DeliveryAddress;
            company.Phone = request.Phone;
            company.Website = request.Website;
            company.BankInfo = request.BankInfo;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 