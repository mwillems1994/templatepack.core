using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace MarcoWillems.Template.MinimalMicroservice.Database.Extensions;

public static class SoftDeleteQueryExtensions
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtensions)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)?
            .MakeGenericMethod(entityData.ClrType);


        var filter = methodToCall?.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression?)filter);

        var index = entityData.
             FindProperty(nameof(ISoftDeletable.Deleted));

        if (index != null)
        {
            entityData.AddIndex(index);
        }
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : class, ISoftDeletable
    {
        Expression<Func<TEntity, bool>> filter = x => x.Deleted == default;
        return filter;
    }
}
