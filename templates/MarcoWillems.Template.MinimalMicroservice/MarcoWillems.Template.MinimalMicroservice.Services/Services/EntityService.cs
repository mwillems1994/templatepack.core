using System;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Models.Entity;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Services;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities;
using MarcoWillems.Template.MinimalMicroservice.Services.Mappers;
using MarcoWillems.Template.MinimalMicroservice.Services.Repositories;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Repositories;
using MassTransit;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Caching;

namespace MarcoWillems.Template.MinimalMicroservice.Services.Services;

public class EntityService : IEntityService
{
    private readonly IEntityRepository _entityRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICacheManager _cacheManager;

    private const string CacheKeyBase = "Entities";

    public EntityService(IEntityRepository entityRepository, IPublishEndpoint publishEndpoint, ICacheManager cacheManager)
    {
        _entityRepository = entityRepository;
        _publishEndpoint = publishEndpoint;
        _cacheManager = cacheManager;
    }

    public async Task<EntityModel> AddEntityAsync(AddEntityModel addEntityModel, CancellationToken cancellationToken = default)
    {
        var entity = addEntityModel.AsEntity();

        await _entityRepository.AddAsync(entity, cancellationToken);

        var entityModel = entity.AsEntityModel();

        var cacheKey = GetCacheKey(entityModel);
        await _cacheManager.SetAsync(cacheKey, entityModel);

        await _publishEndpoint.Publish(entity.AsEntityCreatedEvent(), cancellationToken);

        return entityModel;
    }

    public async Task<EntityModel?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var entityModel = await _entityRepository.GetAsync(x => x.AsEntityModel(), x => x.Id == Id, cancellationToken);

        return entityModel?.FirstOrDefault();
    }

    private static string GetCacheKey(EntityModel entityModel)
    {
        return $"Entities.{entityModel.Id}";
    }
}

