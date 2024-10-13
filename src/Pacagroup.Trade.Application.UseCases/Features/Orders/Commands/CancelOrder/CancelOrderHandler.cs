using MediatR;
using Microsoft.EntityFrameworkCore;
using Pacagroup.Trade.Application.Interfaces.Persistence;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.CancelOrder
{
    internal class CancelOrderHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CancelOrderHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            if (order is not null)
            {
                _applicationDbContext.Orders.Remove(order);
            }

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
