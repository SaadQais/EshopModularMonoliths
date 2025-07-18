namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, string Categories) : 
        IRequest<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
