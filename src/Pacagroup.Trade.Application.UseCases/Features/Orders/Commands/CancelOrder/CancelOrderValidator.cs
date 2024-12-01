using FluentValidation;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
