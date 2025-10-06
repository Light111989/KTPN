using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Companies;

public class Create
{
    public class CreateCompanyCommand : IRequest<Company>
    {
          public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameCode { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string BankInfo { get; set; } = string.Empty;
    public bool IsVirtual { get; set; }
    public List<Contact> Contacts { get; set; } = new();
    }

    // public class Validator // Optionally implement validation logic here
    // {
    //     public Validator() { }
    // }

    public class Handler : IRequestHandler<CreateCompanyCommand, Company>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company
            {
                Code = request.Code,
                Name = request.Name,
                NameCode = request.NameCode,
                TaxCode = request.TaxCode,
                Address = request.Address,
                DeliveryAddress = request.DeliveryAddress,
                Phone = request.Phone,
                Website = request.Website,
                BankInfo = request.BankInfo,
                IsVirtual = request.IsVirtual,
                Contacts = request.Contacts
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync(cancellationToken);

            return company;
        }
    }
} 