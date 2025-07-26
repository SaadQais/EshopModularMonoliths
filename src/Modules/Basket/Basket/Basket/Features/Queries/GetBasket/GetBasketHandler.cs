namespace Basket.Basket.Features.Queries.GetBasket
{
    public record GetBasketQuery(string UserName) 
        : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCartDto ShoppingCart);

    internal class GetBasketHandler(IBasketRepository basketRepository) 
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(
            GetBasketQuery query, 
            CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetAsync(
                query.UserName, 
                asNoTracking: true, 
                cancellationToken);

            return new GetBasketResult(basket.Adapt<ShoppingCartDto>());
        }
    }
}
