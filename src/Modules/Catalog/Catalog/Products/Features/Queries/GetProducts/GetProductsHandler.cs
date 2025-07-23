namespace Catalog.Products.Features.Queries.GetProducts
{
    public record GetProductsQuery(PaginationRequest Request) :
        IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductDto> Products);

    internal class GetProductsHandler(CatalogDbContext context)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(
            GetProductsQuery query, 
            CancellationToken cancellationToken)
        {
            var pageIndex = query.Request.PageIndex;
            var pageSize = query.Request.PageSize;

            var totalCount = await context.Products
                .AsNoTracking()
                .LongCountAsync(cancellationToken);

            var products = await context.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ProjectToType<ProductDto>()
                .ToListAsync(cancellationToken);

            return new GetProductsResult(new PaginatedResult<ProductDto>(
                pageIndex, 
                pageSize, 
                totalCount, 
                products));
        }
    }
}
