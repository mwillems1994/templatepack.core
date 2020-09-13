using System.Security.Claims;
using MarcoWillems.Template.BasicMicroservice.Services.Helpers;
using Microsoft.AspNetCore.Http;

namespace MarcoWillems.Template.BasicMicroservice.API.Helpers
{
    public class UserPrincipalAccessor : IUserPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserPrincipalAccessor(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    }
}
