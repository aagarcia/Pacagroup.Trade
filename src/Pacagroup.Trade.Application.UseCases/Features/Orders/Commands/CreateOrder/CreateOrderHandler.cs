using MediatR;
using AutoMapper;
using Pacagroup.Trade.Application.Interfaces.Persistence;
using Pacagroup.Trade.Domain.Entities;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);

            if (await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
