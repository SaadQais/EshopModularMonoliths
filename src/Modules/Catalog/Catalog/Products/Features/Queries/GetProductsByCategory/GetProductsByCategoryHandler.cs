namespace Catalog.Products.Features.Queries.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(string Category) :
        IQuery<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);

    internal class GetProductsByCategoryHandler(CatalogDbContext context)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(
            GetProductsByCategoryQuery query, 
            CancellationToken cancellationToken)
        {
            var products = await context.Products
                .AsNoTracking()
                .Where(p => p.Categories.Contains(query.Category))
                .OrderBy(p => p.Name)
                .ProjectToType<ProductDto>()
                .ToListAsync(cancellationToken);

            return new GetProductsByCategoryResult(products);
        }
    }
}
