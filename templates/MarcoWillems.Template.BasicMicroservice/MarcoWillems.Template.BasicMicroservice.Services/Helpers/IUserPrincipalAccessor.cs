using System.Security.Claims;

namespace MarcoWillems.Template.BasicMicroservice.Services.Helpers
{
    public interface IUserPrincipalAccessor
    {
        ClaimsPrincipal? User { get; }
    }
}
