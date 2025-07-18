﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

            SeedDataAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

            return app;
        }

        private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider) 
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.MigrateAsync();
        }

        private static async Task SeedDataAsync<TContext>(IServiceProvider serviceProvider) 
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
