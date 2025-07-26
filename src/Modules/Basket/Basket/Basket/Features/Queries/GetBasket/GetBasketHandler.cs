namespace Basket.Basket.Features.Queries.GetBasket
{
    public record GetBasketQuery(string UserName) 
        : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCartDto ShoppingCart);

    internal class GetBasketHandler(BasketDbContext context) 
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(
            GetBasketQuery query, 
            CancellationToken cancellationToken)
        {
            var basket = await context.ShoppingCarts
                .AsNoTracking()
                .Where(b => b.UserName == query.UserName)
                .ProjectToType<ShoppingCartDto>()
                .SingleOrDefaultAsync(cancellationToken)
                    ?? throw new BasketNotFoundException(query.UserName);

            return new GetBasketResult(basket);
        }
    }
}
