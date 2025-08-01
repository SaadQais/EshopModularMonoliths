﻿namespace Catalog.Products.Features.Commands.UpdateProduct
{
    public record UpdateProductRequest(UpdateProductDto Product);

    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder products)
        {
            products.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
        }
    }
}
