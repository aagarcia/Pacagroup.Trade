using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Pacagroup.Trade.Application.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
