using MarcoWillems.Template.WebApi.InputModels.Auth;
using MarcoWillems.Template.WebApi.Services.Contracts.Models.Auth;
using MarcoWillems.Template.WebApi.Services.Contracts.Services;
using MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarcoWillems.Template.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService, 
            ILogger<AuthController> logger
        )
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<ActionResult<AuthToken>> GetTokenAsync([FromForm] GetTokenInputModel inputModel)
        {
            try
            {
                var token = await _authService.GetTokenAsync(inputModel.AsGetTokenModel(), HttpContext.RequestAborted);
                return token;
            }
            catch(UserNotFoundException ex)
            {
                _logger.LogError(ex, "User with username '{0}' tried to login with incorrect credentials", inputModel.UserName);
                return BadRequest("Username of password incorrect");
            }
            catch(PasswordIncorrectException ex)
            {
                _logger.LogError(ex, "User with username '{0}' tried to login with incorrect credentials", inputModel.UserName);
                return BadRequest("Username of password incorrect");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while logging in");
                return StatusCode(500);
            }
        }
    }
}
