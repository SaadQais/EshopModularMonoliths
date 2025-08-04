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

    internal class CheckoutBasketHandler(
        BasketDbContext dbContext,
        IBasketRepository repository,
        IBus bus) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            // get existing basket with total price
            // Set totalprice on basketcheckout event message
            // send basket checkout event to rabbitmq using masstransit
            // delete the basket

            await using var transaction =
                await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var basket = await repository.GetAsync(command.BasketCheckout.UserName, true, cancellationToken);

                var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
                eventMessage.TotalPrice = basket.TotalPrice;

                await bus.Publish(eventMessage, cancellationToken);

                await repository.DeleteAsync(command.BasketCheckout.UserName, cancellationToken);

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
