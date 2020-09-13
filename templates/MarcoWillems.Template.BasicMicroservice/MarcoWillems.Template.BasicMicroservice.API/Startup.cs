using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcoWillems.Template.BasicMicroservice.API.Helpers;
using MarcoWillems.Template.BasicMicroservice.Database.Context;
using MarcoWillems.Template.BasicMicroservice.Services.Extensions;
using MarcoWillems.Template.BasicMicroservice.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pitstop.Infrastructure.Messaging.Configuration;

namespace MarcoWillems.Template.BasicMicroservice.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private const string ConnectionStringName = "<MICROSERVICE_NAME>_CN";
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add DBContext
            var sqlConnectionString = _configuration.GetConnectionString(ConnectionStringName);
            services.AddDbContext<CustomDbContext>(options => 
                options.UseSqlServer(
                    sqlConnectionString, 
                    o => o.MigrationsAssembly("Playground.Nl.CustomerManagementAPI.Database")));

            // add messagepublisher
            services.UseRabbitMQMessagePublisher(_configuration);

            services.AddScoped<IUserPrincipalAccessor, UserPrincipalAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add framework services
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "<MICROSERVICE_NAME> API", Version = "v1" });
            });
            
            services.AddCustomServices();
            
            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                checks.AddSqlCheck(ConnectionStringName, _configuration.GetConnectionString(ConnectionStringName));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}