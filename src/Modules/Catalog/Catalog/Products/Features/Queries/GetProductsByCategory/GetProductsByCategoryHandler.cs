namespace Catalog.Products.Features.Queries.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(GetProductsByCategoryRequest Request) :
        IQuery<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(PaginatedResult<ProductDto> Products);

    internal class GetProductsByCategoryHandler(CatalogDbContext context)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(
            GetProductsByCategoryQuery query, 
            CancellationToken cancellationToken)
        {
            var pageIndex = query.Request.PageIndex;
            var pageSize = query.Request.PageSize;

            var totalCount = await context.Products
                .AsNoTracking()
                .Where(p => p.Categories.Contains(query.Request.Category))
                .LongCountAsync(cancellationToken);

            var products = await context.Products
                .AsNoTracking()
                .Where(p => p.Categories.Contains(query.Request.Category))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .OrderBy(p => p.Name)
                .ProjectToType<ProductDto>()
                .ToListAsync(cancellationToken);

            return new GetProductsByCategoryResult(new PaginatedResult<ProductDto>(
                pageIndex,
                pageSize,
                totalCount,
                products));
        }
    }
}
