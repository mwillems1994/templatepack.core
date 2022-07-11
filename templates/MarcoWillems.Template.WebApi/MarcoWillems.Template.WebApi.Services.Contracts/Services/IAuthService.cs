using MarcoWillems.Template.WebApi.Services.Contracts.Models.Auth;

namespace MarcoWillems.Template.WebApi.Services.Contracts.Services;
public interface IAuthService
{
    Task<AuthToken> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default);
}
