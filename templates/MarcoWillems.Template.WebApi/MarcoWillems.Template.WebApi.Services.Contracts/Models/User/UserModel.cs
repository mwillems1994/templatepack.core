namespace MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? Insertion { get; set; }
    public string LastName { get; set; } = string.Empty;
}
