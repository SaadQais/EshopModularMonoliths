using Catalog.Products.Features.Commands.CreateProduct;
using Catalog.Products.Features.Commands.DeleteProduct;
using Catalog.Products.Features.Commands.UpdateProduct;
using Catalog.Products.Features.Queries.GetProductById;
using Catalog.Products.Features.Queries.GetProducts;
using Catalog.Products.Features.Queries.GetProductsByCategory;

namespace Catalog.Products.Features
{
    public class ProductsModuleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var products = app
                .MapGroup("/products")
                .WithTags("Products");

            CreateProductEndpoint.MapEndpoints(products);
            UpdateProductEndpoint.MapEndpoints(products);
            DeleteProductEndpoint.MapEndpoints(products);
            GetProductsEndpoint.MapEndpoints(products);
            GetProductsByCategoryEndpoint.MapEndpoints(products);
            GetProductByIdEndpoint.MapEndpoints(products);
        }
    }
}
