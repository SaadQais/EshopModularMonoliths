namespace Basket.Basket.Features.Commands.AddItemIntoBasket
{
    public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);

    public record AddItemIntoBasketResponse(Guid Id);

    public class AddItemIntoBasketEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder basket)
        {
            basket.MapPost("/basket/{userName}/item", async (
                [FromRoute] string userName, 
                [FromBody] AddItemIntoBasketRequest request, 
                ISender sender) =>
            {
                var result = await sender.Send(new AddItemIntoBasketCommand(userName, request.ShoppingCartItem));

                var response = result.Adapt<AddItemIntoBasketResponse>();

                return Results.Created($"basket/{response.Id}", response);
            })
            .WithName("AddItemIntoBasket")
            .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Item Into Basket")
            .WithDescription("Add Item Into Basket");
        }
    }
}
