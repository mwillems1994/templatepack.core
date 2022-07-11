

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcoWillems.Template.WebApi.Database.Seeding.Users;
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public static readonly string AdminRoleId = "ea98e2d3-b88a-4ffa-874b-1e6bd66808e2";
    public static readonly string UserRoleId = "5718c9da-5f0f-4f01-9b33-27dd42620509";

    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse(AdminRoleId),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse(UserRoleId),
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
}
