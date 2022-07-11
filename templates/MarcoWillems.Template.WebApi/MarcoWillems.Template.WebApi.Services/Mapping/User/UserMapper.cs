using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Services.Contracts.Models.User;

namespace MarcoWillems.Template.WebApi.Services.Mapping.User;
public static class UserMapper
{
    public static UserModel AsUserModel(this ApplicationUser user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        Insertion = user.Insertion,
        LastName = user.LastName
    };
}
