using Microsoft.Extensions.Hosting;

namespace Ordering
{
    public static class OrderingModule
    {
        public static IHostApplicationBuilder AddOrderingModule(this IHostApplicationBuilder builder)
        {
            // Register interceptors
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventIntercepter>();

            
            builder.AddNpgsqlDbContext<OrderingDbContext>("EShopDb", configureDbContextOptions: options =>
            {
                var sp = builder.Services.BuildServiceProvider();
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            return builder;
        }

        public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
        {
            app.UseMigration<OrderingDbContext>();

            return app;
        }
    }
}
