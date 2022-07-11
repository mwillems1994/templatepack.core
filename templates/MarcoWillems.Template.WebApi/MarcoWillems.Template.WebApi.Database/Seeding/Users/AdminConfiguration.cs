using MarcoWillems.Template.WebApi.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcoWillems.Template.WebApi.Database.Seeding.Users;
public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public static readonly string AdminId = "86bb0820-cbe0-480e-afa7-5806e9725f43";

    public static readonly string SecurityStamp = "7006e09a-9ee9-47bd-a7ef-d53796c73f82";

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var admin = new ApplicationUser
        {
            Id = Guid.Parse(AdminId),
            UserName = "Marco",
            NormalizedUserName = "MARCOWILLEMS",
            FirstName = "Marco",
            LastName = "Willems",
            Email = "mail@marcowillems.nl",
            NormalizedEmail = "MAIL@MARCOWILLEMS.NL",
            PhoneNumber = "0612345678",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.Parse(SecurityStamp).ToString("D"),
        };

        admin.PasswordHash = PassGenerate(admin);

        builder.HasData(admin);
    }

    public static string PassGenerate(ApplicationUser user)
    {
        var passHash = new PasswordHasher<ApplicationUser>();
        return passHash.HashPassword(user, "password");
    }
}