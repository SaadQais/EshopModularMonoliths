namespace Basket.Basket.Features.Commands.AddItemIntoBasket
{
    public record class AddItemIntoBasketCommand(
        string UserName, 
        ShoppingCartItemDto Item) : ICommand<AddItemIntoBasketResult>;

    public record AddItemIntoBasketResult(Guid Id);

    public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
    {
        public AddItemIntoBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Item.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.Item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }

    internal class AddItemIntoBasketHandler(BasketDbContext context)
        : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(
            AddItemIntoBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var shoppingCart = await context.ShoppingCarts
                .Include(s => s.Items)
                .SingleOrDefaultAsync(b => b.UserName == command.UserName, cancellationToken)
                    ?? throw new BasketNotFoundException(command.UserName);

            shoppingCart.AddItem(
                command.Item.ProductId, 
                command.Item.Quantity, 
                command.Item.Color,
                command.Item.Price,
                command.Item.ProductName);

            await context.SaveChangesAsync(cancellationToken);

            return new AddItemIntoBasketResult(shoppingCart.Id);
        }
    }
}
