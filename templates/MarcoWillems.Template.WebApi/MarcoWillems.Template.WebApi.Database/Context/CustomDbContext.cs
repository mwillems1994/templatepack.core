using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Database.Entities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Reflection;

namespace MarcoWillems.Template.WebApi.Database.Context;
public class CustomDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
    {
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityLog
                && (e.State == EntityState.Added
                    || e.State == EntityState.Modified))
            .ToArray();

        foreach (var entry in entries)
        {
            var entity = (EntityLog)entry.Entity;
            entity.Changed = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.Created = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


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
            .Execute(() => Database.Migrate());
    }
}
