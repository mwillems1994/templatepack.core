using MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MarcoWillems.Template.WebApi.InputModels.User
{
    public class AddUserInputModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirm password fields do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? Insertion { get; set; }
        [Required]
        public string LastName { get; set; } = string.Empty;

        public AddUserModel AsAddUserModel() => new()
        {
            UserName = UserName,
            Password = Password,
            FirstName = FirstName,
            Insertion = Insertion,
            LastName = LastName
        };
    }
}
