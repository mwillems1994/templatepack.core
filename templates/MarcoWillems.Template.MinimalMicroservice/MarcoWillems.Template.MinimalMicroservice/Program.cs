using MarcoWillems.Template.MinimalMicroservice;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Caching;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Options;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Repositories;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Services;
using MarcoWillems.Template.MinimalMicroservice.Database.Context;
using MarcoWillems.Template.MinimalMicroservice.Endpoints.Entity;
using MarcoWillems.Template.MinimalMicroservice.Requests.Entity;
using MarcoWillems.Template.MinimalMicroservice.Services.Caching;
using MarcoWillems.Template.MinimalMicroservice.Services.Repositories;
using MarcoWillems.Template.MinimalMicroservice.Services.Services;
using MassTransit;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CustomDbContext>(opt =>
{
    var databaseOptions = builder.Configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();

    if (databaseOptions == null)
    {
        throw new ArgumentNullException(nameof(databaseOptions));
    }

    opt.UseSqlServer(
        databaseOptions.ConnectionString,
        o => o.MigrationsAssembly("MarcoWillems.Template.MinimalMicroservice.Database")
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CachingOptions>(builder.Configuration.GetSection(nameof(CachingOptions)));
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddSingleton<ICacheManager, CacheManager>();

builder.Services.AddOutputCache();

builder.Services.RemoveAll<IOutputCacheStore>();
builder.Services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

builder.Services.AddMassTransit(x =>
{
    var eventingOptions = builder.Configuration.GetSection(nameof(EventingOptions)).Get<EventingOptions>();

    if (eventingOptions == null)
    {
        throw new ArgumentNullException(nameof(eventingOptions));
    }

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(eventingOptions.RabbitMQHost, "/", h =>
        {
            h.Username(eventingOptions.RabbitMQUsername);
            h.Password(eventingOptions.RabbitMQPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseOutputCache();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
    dataContext.MigrateDb();
}

app.AddEntityEndpoints();

app.Run();