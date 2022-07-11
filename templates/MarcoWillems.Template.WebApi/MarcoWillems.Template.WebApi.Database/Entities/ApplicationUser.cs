using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MarcoWillems.Template.WebApi.Database.Entities;
public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string? Insertion { get; set; }
    [Required]
    public string LastName { get; set; } = string.Empty;
}
