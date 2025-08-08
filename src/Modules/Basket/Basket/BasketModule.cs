using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Basket
{
    public static class BasketModule
    {
        public static IHostApplicationBuilder AddBasketModule(this IHostApplicationBuilder builder)
        {
            // Repository services
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

            // Register interceptors
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventIntercepter>();

            builder.AddNpgsqlDbContext<BasketDbContext>("EShopDb", configureDbContextOptions: options =>
            {
                var sp = builder.Services.BuildServiceProvider();
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            // Background service for outbox pattern
            builder.Services.AddHostedService<OutboxProcessor>();

            return builder;
        }

        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            app.UseMigration<BasketDbContext>();

            return app;
        }
    }
}
