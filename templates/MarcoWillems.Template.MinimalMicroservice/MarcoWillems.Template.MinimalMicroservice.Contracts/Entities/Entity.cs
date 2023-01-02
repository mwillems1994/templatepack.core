using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;

namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Entities;

public class Entity : EntityBase
{
    public required string Foo { get; set; } = string.Empty;
    public required int Bar { get; set; }
}

