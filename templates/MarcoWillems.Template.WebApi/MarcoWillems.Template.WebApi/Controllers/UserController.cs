using MarcoWillems.Template.WebApi.InputModels.User;
using MarcoWillems.Template.WebApi.Services.Contracts.Models.User;
using MarcoWillems.Template.WebApi.Services.Contracts.Services;
using MarcoWillems.Template.WebApi.Services.Exceptions.Auth;
using MarcoWillems.Template.WebApi.Services.Exceptions.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarcoWillems.Template.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger
        )
        {
            _userService = userService;
            _logger = logger;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserCreatedResultModel>> CreateUserAsync([FromForm] AddUserInputModel inputModel)
        {
            try
            {
                var result = await _userService.AddUserAsync(inputModel.AsAddUserModel(), HttpContext.RequestAborted);

                if (result.Succeeded)
                {
                    var userModel = await _userService.GetUserAsync(inputModel.UserName);

                    return CreatedAtAction(nameof(GetUserAsync), new { id = userModel.Id }, userModel);
                }

                return BadRequest(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode(500);
            }
        }

        // GET api/<UserController>
        [ActionName(nameof(GetUserAsync))]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserModel>> GetUserAsync(Guid id)
        {
            try
            {
                var result = await _userService.GetUserAsync(id, HttpContext.RequestAborted);

                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return StatusCode(500);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<UserDeletedResultModel>> DeleteUserAsync(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id, HttpContext.RequestAborted);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
