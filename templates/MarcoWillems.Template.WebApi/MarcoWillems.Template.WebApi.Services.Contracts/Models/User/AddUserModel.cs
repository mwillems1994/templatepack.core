namespace MarcoWillems.Template.WebApi.Services.Contracts.Models.User
{
    public class AddUserModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? Insertion { get; set; }
        public string LastName { get; set; } = string.Empty;
    }
}
