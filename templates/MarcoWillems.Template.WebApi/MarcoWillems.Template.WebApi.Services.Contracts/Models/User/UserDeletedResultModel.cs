namespace MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
public record UserDeletedResultModel(bool Succeeded, IEnumerable<string>? Errors = default);