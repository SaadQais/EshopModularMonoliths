namespace Catalog.Products.Features.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) :
        ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
        }
    }

    internal class DeleteProductHandler(CatalogDbContext context)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(
            DeleteProductCommand command, 
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FindAsync([command.ProductId], cancellationToken)
                    ?? throw new ProductNotFoundException(command.ProductId);

            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
