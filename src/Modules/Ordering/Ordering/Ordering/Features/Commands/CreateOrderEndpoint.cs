namespace Ordering.Ordering.Features.Commands
{
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid Id);

    public class CreateOrderEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder orders)
        {
            orders.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Create Order");
        }
    }
}
