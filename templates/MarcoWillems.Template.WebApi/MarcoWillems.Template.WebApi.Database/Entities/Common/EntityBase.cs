using System.ComponentModel.DataAnnotations;

namespace MarcoWillems.Template.WebApi.Database.Entities.Common;
public abstract class EntityBase : EntityLog
{
    [Required, Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
