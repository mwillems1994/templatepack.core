using System;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Repositories;
using MarcoWillems.Template.MinimalMicroservice.Database.Context;

namespace MarcoWillems.Template.MinimalMicroservice.Services.Repositories;

public class EntityRepository : BaseRepository<Entity>, IEntityRepository
{
    public EntityRepository(CustomDbContext dbContext) : base(dbContext)
    {
    }
}
