using MarcoWillems.Template.WebApi.Services.Contracts.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace MarcoWillems.Template.WebApi.InputModels.Auth;
public class GetTokenInputModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public GetTokenModel AsGetTokenModel() => new(UserName, Password);
}
