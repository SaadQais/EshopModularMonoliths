using Basket.Basket.Features.Commands.AddItemIntoBasket;
using Basket.Basket.Features.Commands.CheckoutBasket;
using Basket.Basket.Features.Commands.CreateBasket;
using Basket.Basket.Features.Commands.DeleteBasket;
using Basket.Basket.Features.Commands.RemoveItemFromBasket;
using Basket.Basket.Features.Queries.GetBasket;

namespace Basket.Basket.Features
{
    public class BasketModuleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var basket = app
                .MapGroup("/basket")
                .WithTags("Basket")
                .RequireAuthorization();

            GetBasketEndpoint.MapEndpoints(basket);
            CreateBasketEndpoint.MapEndpoints(basket);
            DeleteBasketEndpoint.MapEndpoints(basket);
            AddItemIntoBasketEndpoint.MapEndpoints(basket);
            RemoveItemFromBasketEndpoint.MapEndpoints(basket);
            CheckoutBasketEndpoint.MapEndpoints(basket);
        }
    }
}
