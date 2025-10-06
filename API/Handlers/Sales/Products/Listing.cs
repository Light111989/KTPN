using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Products;

public class Listing
{
    public class ListProductsQuery : IRequest<IEnumerable<Product>>
    {
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<ListProductsQuery, IEnumerable<Product>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }
    }
} 