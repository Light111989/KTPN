using System;
using API.Data;
using API.Domain;
using MediatR;

namespace API.Handlers.BienCheListing;

public class GetById
{
    public class GetBienCheByIdQuery : IRequest<BienChe>
    {
        public Guid Id { get; set; }
    }

    public class Validator // Optionally implement validation logic here
    {
        public Validator() { }
    }
    public class Handler : IRequestHandler<GetBienCheByIdQuery, BienChe?>
    {
        private readonly ApplicationDbContext _context;
        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BienChe?> Handle(GetBienCheByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.BienChes.FindAsync(request.Id);
        }
    }
}

