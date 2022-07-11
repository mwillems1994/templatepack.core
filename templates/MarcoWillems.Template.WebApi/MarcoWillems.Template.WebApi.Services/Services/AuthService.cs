using MarcoWillems.Template.WebApi.Database.Entities;
using MarcoWillems.Template.WebApi.Services.Contracts.Models.Auth;
using MarcoWillems.Template.WebApi.Services.Contracts.Services;
using MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarcoWillems.Template.WebApi.Services.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<AuthToken> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            throw new UserNotFoundException(model.UserName);
        }

        if(!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new PasswordIncorrectException();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = GetClaims(user, userRoles.ToList());

        var token = GetToken(authClaims);

        return token;
    }

    private static IEnumerable<Claim> GetClaims(ApplicationUser user, List<string> roles)
    {
        yield return new Claim(ClaimTypes.Name, user.UserName);
        foreach(var role in roles)
        {
            yield return new Claim(ClaimTypes.Role, role);
        }
    }

    private static AuthToken GetToken(IEnumerable<Claim> authClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("PeiOdQ0GsvAibpK6Hv4oE1JjlzFSlPs1");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var accessToken = tokenHandler.WriteToken(token);

        return new AuthToken(accessToken);
    }
}
