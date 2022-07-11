using MarcoWillems.Template.WebApi.Database.Context;
using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Services.Contracts.Repositories;
using MarcoWillems.Template.WebApi.Services.Contracts.Services;
using MarcoWillems.Template.WebApi.Services.Repositories;
using MarcoWillems.Template.WebApi.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MarcoWillems.Template.WebApi
{
    internal static partial class ProgramExtensions
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            services.AddDbContext<CustomDbContext>(options =>
                options.UseNpgsql("Server=db;Port=5432;Database=database;User Id=postgres;Password=postgres", o => o.MigrationsAssembly("MarcoWillems.Template.WebApi.Database")));

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CustomDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Only for development purposes, should validate this stuff when using in prod environment
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PeiOdQ0GsvAibpK6Hv4oE1JjlzFSlPs1"))
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });

            return services;
        }

        public static void MigrateDatabase(this IServiceProvider services)
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            scope!.ServiceProvider!.GetService<CustomDbContext>()!.MigrateDb();
        }
    }
}
