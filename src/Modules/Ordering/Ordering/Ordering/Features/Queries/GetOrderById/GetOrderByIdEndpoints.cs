namespace Ordering.Ordering.Features.Queries.GetOrderById
{
    public record GetOrderByIdResponse(OrderDto Order);

    public class GetOrderByIdEndpoints
    {
        public static void MapEndpoints(RouteGroupBuilder orders)
        {
            orders.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByIdQuery(id));

                var response = result.Adapt<GetOrderByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrderById")
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Order By Id")
            .WithDescription("Get Order By Id");
        }
    }
}
