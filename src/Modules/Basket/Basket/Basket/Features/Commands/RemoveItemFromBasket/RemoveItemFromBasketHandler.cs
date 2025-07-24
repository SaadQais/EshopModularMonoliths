namespace Basket.Basket.Features.Commands.RemoveItemFromBasket
{
    public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
        : ICommand<RemoveItemFromBasketResult>;

    public record RemoveItemFromBasketResult(Guid Id);

    public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
    {
        public RemoveItemFromBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        }
    }

    internal class RemoveItemFromBasketHandler(BasketDbContext context)
        : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        public async Task<RemoveItemFromBasketResult> Handle(
            RemoveItemFromBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var shoppingCart = await context.ShoppingCarts
                .Include(s => s.Items)
                .SingleOrDefaultAsync(b => b.UserName == command.UserName, cancellationToken)
                    ?? throw new BasketNotFoundException(command.UserName);

            shoppingCart.RemoveItem(command.ProductId);

            await context.SaveChangesAsync(cancellationToken);

            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
}
