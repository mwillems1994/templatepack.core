namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;

public interface ISoftDeletable
{
    public DateTime? Deleted { get; set; }
}

