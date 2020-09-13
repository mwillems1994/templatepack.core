using System;

namespace MarcoWillems.Template.BasicMicroservice.Database.Common
{
    public abstract class EntityLog
    {
        public DateTime? Deleted { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Changed { get; set; } = DateTime.Now;
    }
}
