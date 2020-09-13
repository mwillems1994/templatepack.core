using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarcoWillems.Template.BasicMicroservice.Database.Common;
using Microsoft.EntityFrameworkCore;

namespace MarcoWillems.Template.BasicMicroservice.Database.Context
{
    public class CustomDbContext : DbContext
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
                entity.Changed = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.Created = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
