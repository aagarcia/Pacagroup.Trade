using FluentValidation;

namespace Pacagroup.Trade.Application.UseCases.Features.Orders.Queries.GetOrder
{
    public class GetOrderValidator : AbstractValidator<GetOrderQuery>
    {
        public GetOrderValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
