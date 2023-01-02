using System;
namespace MarcoWillems.Template.MinimalMicroservice.Requests.Entity;

public class AddEntityRequest
{
    public string Foo { get; set; } = string.Empty;
    public int Bar { get; set; }
}

