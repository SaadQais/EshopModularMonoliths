namespace Ordering.Ordering.Features.Queries.GetOrders
{
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

    public class GetOrdersEndpoints
    {
        public static void MapEndpoints(RouteGroupBuilder orders)
        {
            orders.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));

                GetOrdersResponse response = result.Adapt<GetOrdersResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders")
            .WithDescription("Get Orders");
        }
    }
}
