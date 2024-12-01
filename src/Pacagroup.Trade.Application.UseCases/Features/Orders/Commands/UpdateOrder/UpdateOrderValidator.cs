using FluentValidation;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.Quanty).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Price).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
