namespace Catalog.Products.Features.Queries.GetProductsByCategory
{
    public record GetProductsByCategoryRequest(string Category, int PageIndex = 0, int PageSize = 10)
        : PaginationRequest(PageIndex, PageSize);

    public record GetProductsByCategoryResponse(PaginatedResult<ProductDto> Products);

    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category", async ([AsParameters] GetProductsByCategoryRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(request));

                var response = result.Adapt<GetProductsByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}
