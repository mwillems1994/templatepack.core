using MarcoWillems.Template.MinimalMicroservice.Contracts.Models.Entity;
using MarcoWillems.Template.MinimalMicroservice.Requests.Entity;

namespace MarcoWillems.Template.MinimalMicroservice.Mappers;

public static class EntityMapper
{
    public static AddEntityModel AsAddEntityModel(this AddEntityRequest addEntityInputModel)
    {
        var addEntityModel = new AddEntityModel(addEntityInputModel.Foo, addEntityInputModel.Bar);

        return addEntityModel;
    }
}
