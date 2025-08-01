﻿namespace Catalog.Products.Features.Queries.GetProducts
{
    //public record GetProductsRequest(PaginationRequest Request);

    public record GetProductsResponse(PaginatedResult<ProductDto> Products);

    public class GetProductsEndpoint
    {
        public static void MapEndpoints(RouteGroupBuilder products)
        {
            products.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(request));
                
                var response = result.Adapt<GetProductsResponse>(); 

                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
