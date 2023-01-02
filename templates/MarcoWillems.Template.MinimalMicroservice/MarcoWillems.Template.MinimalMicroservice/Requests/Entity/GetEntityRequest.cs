using System;
using Microsoft.AspNetCore.OutputCaching;

namespace MarcoWillems.Template.MinimalMicroservice.Requests.Entity;

public class GetEntityRequest
{
    public Guid Id { get; init; }
}
