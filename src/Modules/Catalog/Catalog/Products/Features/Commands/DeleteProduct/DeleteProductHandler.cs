namespace Catalog.Products.Features.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) :
        ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductHandler(CatalogDbContext context)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(
            DeleteProductCommand command, 
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FindAsync([command.Id], cancellationToken)
                    ?? throw new Exception($"Product not found: {command.Id}");

            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
