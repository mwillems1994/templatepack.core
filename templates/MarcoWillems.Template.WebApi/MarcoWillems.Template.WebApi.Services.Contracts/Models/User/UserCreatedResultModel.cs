namespace MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
public record UserCreatedResultModel(bool Succeeded, IEnumerable<string>? Errors = default);
