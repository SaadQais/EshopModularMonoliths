using Ordering.Ordering.Features.Commands.CreateOrder;
using Ordering.Ordering.Features.Commands.DeleteOrder;
using Ordering.Ordering.Features.Queries.GetOrderById;
using Ordering.Ordering.Features.Queries.GetOrders;

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
            DeleteOrderEndpoints.MapEndpoints(orders);
            GetOrdersEndpoints.MapEndpoints(orders);
            GetOrderByIdEndpoints.MapEndpoints(orders);
        }
    }
}
