using MarcoWillems.Template.WebApi.Services.Contracts.Models.User;

namespace MarcoWillems.Template.WebApi.Services.Contracts.Services;
public interface IUserService
{
    Task<UserCreatedResultModel> AddUserAsync(AddUserModel model, CancellationToken cancellationToken = default);

    Task<UserModel> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserModel> GetUserAsync(string userName, CancellationToken cancellationToken = default);

    Task<UserDeletedResultModel> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}