using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Database.Entities.Common;
using MarcoWillems.Template.WebApi.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MarcoWillems.Template.WebApi.Database.Context
{
    public static class CustomDbContextExtensions
    {
        public static ModelBuilder ConfigureEntities(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                    .ToTable(nameof(ApplicationUser));

            return builder;
        }

        public static ModelBuilder ConfigureSoftDeletable(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

            return builder;
        }
    }
}
