using MarcoWillems.Template.MinimalMicroservice.Contracts.Models.Entity;

namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Services;

public interface IEntityService
{
    Task<EntityModel> AddEntityAsync(AddEntityModel addEntityModel, CancellationToken cancellationToken = default);
    Task<EntityModel?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
}

