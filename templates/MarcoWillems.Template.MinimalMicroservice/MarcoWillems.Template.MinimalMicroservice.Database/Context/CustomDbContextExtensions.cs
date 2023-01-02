using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;
using MarcoWillems.Template.MinimalMicroservice.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MarcoWillems.Template.MinimalMicroservice.Database.Context;

public static class CustomDbContextExtensions
{
    public static ModelBuilder ConfigureEntities(this ModelBuilder builder)
    {
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

