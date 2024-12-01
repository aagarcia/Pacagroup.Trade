using AutoMapper;
using Grpc.Core;
using MediatR;
using Pacagroup.Trade.Services.gRPC.Protos;
using Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.CreateOrder;
using Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.UpdateOrder;
using Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.CancelOrder;
using Pacagroup.Trade.Application.UseCases.Features.Orders.Queries.GetOrder;
using Pacagroup.Trade.Application.UseCases.Features.Orders.Queries.GetAllOrder;

namespace Pacagroup.Trade.Services.gRPC.Services
{
    public class OrderService : Order.OrderBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<GetAllOrdersResponse> GetAllOrders(GetAllOrdersRequest request, ServerCallContext context)
        {
            var ordersDto = await _mediator.Send(new GetAllOrderQuery());
            var response = new GetAllOrdersResponse();
            var serverResponse = new ServerResponse();

            if (ordersDto.Any())
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Consulta Existosa!!!";

                response.Data.AddRange(_mapper.Map<IEnumerable<OrderResponse>>(ordersDto));
            }
            else
            {
                serverResponse.Message = "No se encontraron Ordenes!!!";
            }

            response.ServerResponse = serverResponse;

            return response;
        }

        public override async Task<GetOrderResponse> GetOrder(GetOrderRequest getOrderRequest, ServerCallContext context)
        {
            var orderDto = await _mediator.Send(new GetOrderQuery() { Id = getOrderRequest.Id });
            var response = new GetOrderResponse();
            var serverResponse = new ServerResponse();

            if (orderDto is null)
            {
                serverResponse.Message = $"No se encontró la Orden #: {getOrderRequest.Id}";

                response.ServerResponse = serverResponse;
                return response;
            }
            
            response.Data = _mapper.Map<OrderResponse>(orderDto);
            serverResponse.IsSuccess = true;
            serverResponse.Message = "Consulta Existosa!!!";

            response.ServerResponse = serverResponse;

            return response;
        }

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest createOrderRequest, ServerCallContext context)
        {
            var createOrderCommand = _mapper.Map<CreateOrderCommand>(createOrderRequest);
            bool status = await _mediator.Send(createOrderCommand);
            var response = new CreateOrderResponse();
            var serverResponse = new ServerResponse();

            if (status)
            {
                var orderDto = await _mediator.Send(new GetOrderQuery() { Id = createOrderRequest.Id });

                response.Data = _mapper.Map<OrderResponse>(orderDto);
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Registro Exitoso!!!";
            }
            else
            {
                serverResponse.Message = $"Errores al crear la Order #: {createOrderRequest.Id}";
            }

            response.ServerResponse = serverResponse;

            return response;
        }

        public override async Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest updateOrderRequest, ServerCallContext context)
        {
            var updateOrderCommand = _mapper.Map<UpdateOrderCommand>(updateOrderRequest);
            bool status = await _mediator.Send(updateOrderCommand);
            var response = new UpdateOrderResponse();
            var serverResponse = new ServerResponse();

            if (status)
            {
                var orderDto = await _mediator.Send(new GetOrderQuery() { Id = updateOrderRequest.Id });

                response.Data = _mapper.Map<OrderResponse>(orderDto);
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Actualización Exitosa!!!";
            }
            else
            {
                serverResponse.Message = $"Errores al actualizar la Order #: {updateOrderRequest.Id}";
            }

            response.ServerResponse = serverResponse;

            return response;
        }

        public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest cancelOrderRequest, ServerCallContext context)
        {
            bool status = await _mediator.Send(new CancelOrderCommand() { Id = cancelOrderRequest.Id });
            var response = new CancelOrderResponse();
            var serverResponse = new ServerResponse();

            if (status)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Eliminación Exitosa!!!";
            }
            else
            {
                serverResponse.Message = $"Errores al eliminar la Order #: {cancelOrderRequest.Id}";
            }

            response.ServerResponse = serverResponse;

            return response;
        }
    }
}
