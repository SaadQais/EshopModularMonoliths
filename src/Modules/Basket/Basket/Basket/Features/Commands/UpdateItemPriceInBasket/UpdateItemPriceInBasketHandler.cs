namespace Basket.Basket.Features.Commands.UpdateItemPriceInBasket
{
    public record UpdateItemPriceInBasketCommand(
        Guid ProductId, 
        decimal Price) : ICommand<UpdateItemPriceInBasketResult>;

    public record UpdateItemPriceInBasketResult(bool IsSuccess);

    public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
    {
        public UpdateItemPriceInBasketCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateItemPriceInBasketHandler(BasketDbContext context)
        : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
    {
        public async Task<UpdateItemPriceInBasketResult> Handle(
            UpdateItemPriceInBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var itemsToUpdate = await context.ShoppingCartItems
              .Where(x => x.ProductId == command.ProductId)
              .ToListAsync(cancellationToken);

            if (itemsToUpdate.Count == 0)
            {
                return new UpdateItemPriceInBasketResult(false);
            }

            foreach (var item in itemsToUpdate)
            {
                item.UpdatePrice(command.Price);
            }

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
