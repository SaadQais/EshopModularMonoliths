namespace Basket.Basket.Features.Commands.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) 
        : ICommand<DeletBasketResult>;

    public record DeletBasketResult(bool IsSuccess);

    internal class DeleteBasketHandler(IBasketRepository basketRepository)
        : ICommandHandler<DeleteBasketCommand, DeletBasketResult>
    {
        public async Task<DeletBasketResult> Handle(
            DeleteBasketCommand command, 
            CancellationToken cancellationToken)
        {
            await basketRepository.DeleteAsync(command.UserName, cancellationToken);

            return new DeletBasketResult(true);
        }
    }
}
