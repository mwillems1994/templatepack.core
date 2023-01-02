using System.Reflection;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

using Polly;

namespace MarcoWillems.Template.MinimalMicroservice.Database.Context;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options)
        : base(options) { }

    public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureEntities();

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.ConfigureSoftDeletable();
    }

    public void MigrateDb()
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(3, r => TimeSpan.FromSeconds(10))
            .Execute(() => {
                Database.Migrate();
            });
    }
}