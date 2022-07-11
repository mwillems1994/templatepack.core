namespace MarcoWillems.Template.WebApi.Database.Entities.Common;

public interface ISoftDeletable
{
    public DateTime? Deleted { get; set; }
}
