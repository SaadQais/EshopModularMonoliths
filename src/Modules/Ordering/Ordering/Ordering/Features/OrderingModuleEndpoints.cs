using Ordering.Ordering.Features.Commands;

namespace Ordering.Ordering.Features
{
    public class OrderingModuleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var orders = app
                .MapGroup("/orders")
                .WithTags("Orders");

            CreateOrderEndpoint.MapEndpoints(orders);
        }
    }
}
