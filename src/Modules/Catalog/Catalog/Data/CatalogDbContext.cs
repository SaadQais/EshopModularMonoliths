﻿using System.Reflection;

namespace Catalog.Data
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("catalog");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
