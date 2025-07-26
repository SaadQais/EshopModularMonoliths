namespace Catalog.Products.Features.Commands.CreateProduct
{
    public record CreateProductRequest(CreateProductDto Product);

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder products)
        {
            products.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        }
    }
}
