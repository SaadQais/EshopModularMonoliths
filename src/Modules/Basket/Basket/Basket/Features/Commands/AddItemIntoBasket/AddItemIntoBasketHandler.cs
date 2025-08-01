﻿namespace Basket.Basket.Features.Commands.AddItemIntoBasket
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

    internal class AddItemIntoBasketHandler(
        IBasketRepository basketRepository,
        ISender sender) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(
            AddItemIntoBasketCommand command, 
            CancellationToken cancellationToken)
        {
            var shoppingCart = await basketRepository.GetAsync(
                command.UserName, 
                false, 
                cancellationToken);

            var result = await sender
                .Send(new GetProductByIdQuery(command.Item.ProductId), cancellationToken);

            shoppingCart.AddItem(
                command.Item.ProductId, 
                command.Item.Quantity, 
                command.Item.Color,
                result.Product.Price,
                result.Product.Name);

            await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

            return new AddItemIntoBasketResult(shoppingCart.Id);
        }
    }
}
