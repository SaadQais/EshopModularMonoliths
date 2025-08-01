﻿namespace Shared.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next(cancellationToken);
            }

            var validationContext = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

            var failures =
                validationResults
                .Where(r => r.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);

            return await next(cancellationToken);
        }
    }
}
