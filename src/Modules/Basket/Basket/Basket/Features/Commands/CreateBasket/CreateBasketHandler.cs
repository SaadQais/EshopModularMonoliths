namespace Basket.Basket.Features.Commands.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart) 
        : ICommand<CreateBasketResult>;

    public record CreateBasketResult(Guid Id);

    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class CreateBasketHandler(IBasketRepository basketRepository)
        : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(
            CreateBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var shoppingCart = CreateNewBasket(command.ShoppingCart);

            await basketRepository.CreateAsync(shoppingCart, cancellationToken);

            return new CreateBasketResult(shoppingCart.Id);
        }

        private static ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart =  ShoppingCart.Create(shoppingCartDto.UserName);

            foreach (var item in shoppingCartDto.Items)
            {
                shoppingCart.AddItem(
                    item.ProductId, 
                    item.Quantity, 
                    item.Color, 
                    item.Price, 
                    item.ProductName);
            }

            return shoppingCart;
        }
    }
}
