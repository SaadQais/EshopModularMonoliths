namespace Basket.Basket.Features.Commands.CreateBasket
{
    public record CreateBasketRequest(ShoppingCartDto ShoppingCart);

    public record CreateBasketResponse(Guid Id);

    public class CreateBasketEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder basket)
        {
            basket.MapPost("/basket", async (
                CreateBasketRequest request, 
                ISender sender,
                ClaimsPrincipal user) =>
            {
                var userName = user.Identity!.Name;
                var updatedShoppingCart = request.ShoppingCart with { UserName = userName! };

                var command = new CreateBasketCommand(updatedShoppingCart);

                var result = await sender.Send(command);

                var response = result.Adapt<CreateBasketResponse>();

                return Results.Created($"basket/{response.Id}", response);
            })
            .WithName("CreateBasket")
            .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Create Basket")
            .WithDescription("Create Basket");
        }
    }
}
