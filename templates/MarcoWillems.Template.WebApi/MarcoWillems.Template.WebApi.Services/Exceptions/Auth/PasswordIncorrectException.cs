namespace MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
public class PasswordIncorrectException : Exception
{
    public PasswordIncorrectException() : base("The prodived password is incorrect") { }
}
