namespace Catalog.Products.Features.Queries.GetProductById
{
    internal class GetProductByIdHandler(CatalogDbContext context)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken)
                    ?? throw new ProductNotFoundException(query.Id);

            return new GetProductByIdResult(product.Adapt<ProductDto>());
        }
    }
}
