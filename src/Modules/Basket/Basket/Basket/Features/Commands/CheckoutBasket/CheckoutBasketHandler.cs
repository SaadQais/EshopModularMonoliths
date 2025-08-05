namespace Basket.Basket.Features.Commands.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
        : ICommand<CheckoutBasketResult>;

    public record CheckoutBasketResult(bool IsSuccess);
    
    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class CheckoutBasketHandler(BasketDbContext context) 
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Get existing basket with total price
                var basket = await context.ShoppingCarts
                    .Include(x => x.Items)
                    .SingleOrDefaultAsync(x => x.UserName == command.BasketCheckout.UserName, cancellationToken) 
                        ?? throw new BasketNotFoundException(command.BasketCheckout.UserName);

                // Set total price on basket checkout event message
                var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
                eventMessage.TotalPrice = basket.TotalPrice;

                // Write a message to the outbox
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                    Content = JsonSerializer.Serialize(eventMessage),
                    OccuredOn = DateTime.UtcNow
                };

                await context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

                // Delete the basket
                context.ShoppingCarts.Remove(basket);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new CheckoutBasketResult(true);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CheckoutBasketResult(false);
            }
        }
    }
}
