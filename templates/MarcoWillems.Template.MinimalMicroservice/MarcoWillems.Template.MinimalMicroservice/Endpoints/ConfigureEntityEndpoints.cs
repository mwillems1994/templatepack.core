using System;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Services;
using MarcoWillems.Template.MinimalMicroservice.Mappers;
using MarcoWillems.Template.MinimalMicroservice.Requests.Entity;
using MarcoWillems.Template.MinimalMicroservice.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarcoWillems.Template.MinimalMicroservice.Endpoints.Entity
{
    public record Test(TimeOnly TimeOnly);

	public static class ConfigureEntityEndpoints
    {

		public static void AddEntityEndpoints(this WebApplication app)
		{
            app.MapPost("/Entities", async ([FromBody]AddEntityRequest request, IEntityService entityService, CancellationToken ct) =>
            {
                var addEntityModel = request.AsAddEntityModel();

                var entityModel = await entityService.AddEntityAsync(addEntityModel, ct);

                return Results.Created($"/Entities/{entityModel.Id}", entityModel);
            });

            app.MapGet("/Entities/{id}", async (Guid id, IEntityService entityService, CancellationToken ct) =>
            {
                var entityModel = await entityService.GetAsync(id, ct);

                if (entityModel is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(entityModel);
            }).CacheOutput();
        }
	}
}

