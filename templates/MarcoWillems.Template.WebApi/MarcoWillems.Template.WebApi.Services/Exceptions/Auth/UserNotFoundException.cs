namespace MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName) : base($"User with username '{userName}' cannot be found") { }

    public UserNotFoundException(Guid userId) : base($"User with id '{userId}' cannot be found") { }

}
