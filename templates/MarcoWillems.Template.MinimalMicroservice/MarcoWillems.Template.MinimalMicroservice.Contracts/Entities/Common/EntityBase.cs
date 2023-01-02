using System.ComponentModel.DataAnnotations;

namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;

public abstract class EntityBase : EntityLog
{
    [Required, Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}

