namespace Basket.Basket.Features.Queries.GetBasket
{
    public record GetBasketResponse(ShoppingCartDto ShoppingCart);

    public class GetBasketEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder basket)
        {
            basket.MapGet("/basket/{username}", async (string username, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(username));

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
        }
    }
}
