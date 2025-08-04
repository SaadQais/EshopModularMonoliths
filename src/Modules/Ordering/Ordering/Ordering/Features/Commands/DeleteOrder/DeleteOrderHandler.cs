using Ordering.Ordering.Exceptions;

namespace Ordering.Ordering.Features.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId)
        : ICommand<DeleteOrderResult>;

    public record DeleteOrderResult(bool IsSuccess);
    
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderName is required");
        }
    }

    internal class DeleteOrderHandler(OrderingDbContext dbContext)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders
               .FindAsync([command.OrderId], cancellationToken: cancellationToken) 
                    ?? throw new OrderNotFoundException(command.OrderId);

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return new DeleteOrderResult(true);
        }
    }
}
