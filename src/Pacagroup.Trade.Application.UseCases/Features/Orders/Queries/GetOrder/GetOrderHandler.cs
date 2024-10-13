using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pacagroup.Trade.Application.Interfaces.Persistence;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Queries.GetOrder
{
    internal class GetOrderHandler : IRequestHandler<GetOrderQuery, GetOrderResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetOrderHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GetOrderResponseDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            var response = _mapper.Map<GetOrderResponseDto>(order);

            return response;
        }
    }
}
