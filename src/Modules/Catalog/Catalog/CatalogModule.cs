using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IHostApplicationBuilder AddCatalogModule(this IHostApplicationBuilder builder)
        {
            // Register interceptors first
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventIntercepter>();

            // Add PostgreSQL connection
            builder.AddNpgsqlDbContext<CatalogDbContext>("EShopDb", configureDbContextOptions: options =>
            {
                var sp = builder.Services.BuildServiceProvider();
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            // Register your data seeder
            builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();

            return builder;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            app.UseMigration<CatalogDbContext>();

            return app;
        }
    }
}
