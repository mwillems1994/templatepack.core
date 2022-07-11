using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
using MarcoWillems.Template.WebApi.Services.Contracts.Services;
using MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
using MarcoWillems.Template.WebApi.Services.Exceptions.User;
using MarcoWillems.Template.WebApi.Services.Mapping.User;
using Microsoft.AspNetCore.Identity;

namespace MarcoWillems.Template.WebApi.Services.Services;
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserCreatedResultModel> AddUserAsync(AddUserModel model, CancellationToken cancellationToken = default)
    {
        var userExists = (await _userManager.FindByNameAsync(model.UserName)) != null;

        if(userExists)
        {
            throw new UserAlreadyExistsException(model.UserName);
        }

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            Insertion = model.Insertion,
            LastName = model.LastName
        };

        user.PasswordHash = GeneratePassword(user, model.Password);

        var result = await _userManager.CreateAsync(user);

        if(result == null || !result.Succeeded || result.Errors.Any())
        {
            return new UserCreatedResultModel(false, result?.Errors.Select(item => item.Description));
        }
        
        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if(roleResult == null || !roleResult.Succeeded || result.Errors.Any())
        {
            return new UserCreatedResultModel(false, roleResult?.Errors.Select(item => item.Description));
        }

        return new UserCreatedResultModel(true);
    }

    public async Task<UserModel> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var model = user.AsUserModel();

        return model;
    }

    public async Task<UserModel> GetUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }

        var model = user.AsUserModel();

        return model;
    }

    public async Task<UserDeletedResultModel> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await _userManager.DeleteAsync(user);

        if (result == null || !result.Succeeded || result.Errors.Any())
        {
            return new UserDeletedResultModel(false, result?.Errors.Select(item => item.Description));
        }

        return new UserDeletedResultModel(true);
    }

    private static string GeneratePassword(ApplicationUser user, string password)
    {
        var passHash = new PasswordHasher<ApplicationUser>();
        return passHash.HashPassword(user, password);
    }
}