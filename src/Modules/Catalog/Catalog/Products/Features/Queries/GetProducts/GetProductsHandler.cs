namespace Catalog.Products.Features.Queries.GetProducts
{
    public record GetProductsQuery() :
        IQuery<GetProductsResult>;

    public record GetProductsResult(IEnumerable<ProductDto> Products);

    internal class GetProductsHandler(CatalogDbContext context)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(
            GetProductsQuery query, 
            CancellationToken cancellationToken)
        {
            var products = await context.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ProjectToType<ProductDto>()
                .ToListAsync(cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
