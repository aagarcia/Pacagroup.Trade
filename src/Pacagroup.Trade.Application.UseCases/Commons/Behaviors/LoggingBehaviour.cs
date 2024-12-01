using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Pacagroup.Trade.Application.UseCases.Commons.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, 
                                      RequestHandlerDelegate<TResponse> next, 
                                      CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();

            _logger.LogInformation("Clean Architecture Request Handling: { @correlationId } { name } { @request }",
                                   correlationId,
                                   typeof(TRequest).Name, 
                                   JsonSerializer.Serialize(request));

            var response = await next();

            _logger.LogInformation("Clean Architecture Request Handling: { @correlationId } { name } { @response }",
                                   correlationId,
                                   typeof(TResponse).Name,
                                   JsonSerializer.Serialize(response));

            return response;
        }
    }
}
