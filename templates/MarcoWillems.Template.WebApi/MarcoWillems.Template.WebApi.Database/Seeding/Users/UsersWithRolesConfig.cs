using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcoWillems.Template.WebApi.Database.Seeding.Users;

public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        IdentityUserRole<Guid> iurAdmin = new IdentityUserRole<Guid>
        {
            RoleId = Guid.Parse(RoleConfiguration.AdminRoleId),
            UserId = Guid.Parse(AdminConfiguration.AdminId)
        };

        IdentityUserRole<Guid> iurUser = new IdentityUserRole<Guid>
        {
            RoleId = Guid.Parse(RoleConfiguration.UserRoleId),
            UserId = Guid.Parse(AdminConfiguration.AdminId)
        };

        builder.HasData(iurAdmin);
        builder.HasData(iurUser);
    }
}
