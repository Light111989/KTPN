using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Products;

public class GetById
{
    public class GetProductByIdQuery : IRequest<Product?>
    {
        public int Id { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(request.Id);
        }
    }
} 