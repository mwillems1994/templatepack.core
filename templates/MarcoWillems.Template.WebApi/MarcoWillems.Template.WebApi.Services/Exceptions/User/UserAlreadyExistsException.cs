namespace MarcoWillems.Template.WebApi.Services.Exceptions.User;
public  class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string username) : base($"User with username '{username}' already exists") { }
}