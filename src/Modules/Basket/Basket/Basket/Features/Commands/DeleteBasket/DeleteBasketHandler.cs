namespace Basket.Basket.Features.Commands.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) 
        : ICommand<DeletBasketResult>;

    public record DeletBasketResult(bool IsSuccess);

    internal class DeleteBasketHandler(BasketDbContext context)
        : ICommandHandler<DeleteBasketCommand, DeletBasketResult>
    {
        public async Task<DeletBasketResult> Handle(
            DeleteBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var basket = await context.ShoppingCarts
                .SingleOrDefaultAsync(b => b.UserName == command.UserName, cancellationToken)
                    ?? throw new BasketNotFoundException(command.UserName);

            context.ShoppingCarts.Remove(basket);
            await context.SaveChangesAsync(cancellationToken);

            return new DeletBasketResult(true);
        }
    }
}
