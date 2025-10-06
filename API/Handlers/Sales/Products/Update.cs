using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.Sales.Products;

public class Update
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }

    public class Handler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _context.Products.FindAsync(request.Id);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with ID {request.Id} not found.");
            }

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.StockQuantity = request.StockQuantity;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return existingProduct;
        }
    }
} 