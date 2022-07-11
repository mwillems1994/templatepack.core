namespace MarcoWillems.Template.WebApi.Database.Entities.Common;

public abstract class EntityLog : ISoftDeletable
{
    public DateTime? Deleted { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Changed { get; set; } = DateTime.UtcNow;
}
