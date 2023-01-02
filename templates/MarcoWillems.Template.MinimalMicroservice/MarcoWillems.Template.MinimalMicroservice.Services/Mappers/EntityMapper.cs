using MarcoWillems.Template.MinimalMicroservice.Contracts.Models.Entity;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Events;

namespace MarcoWillems.Template.MinimalMicroservice.Services.Mappers;

public static class EntityMapper
{
    public static Entity AsEntity(this AddEntityModel addEntityModel)
    {
        var entity = new Entity
        {
            Foo = addEntityModel.Foo,
            Bar = addEntityModel.Bar
        };

        return entity;
    }

    public static EntityModel AsEntityModel(this Entity entity)
    {
        var entityModel = new EntityModel(
            entity.Id,
            entity.Foo,
            entity.Bar
        );

        return entityModel;
    }

    public static EntityCreatedEvent AsEntityCreatedEvent(this Entity entity)
    {
        var entityCreatedEvent = new EntityCreatedEvent(
            entity.Foo,
            entity.Bar
        );

        return entityCreatedEvent;
    }
}