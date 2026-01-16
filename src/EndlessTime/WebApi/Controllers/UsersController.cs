using Common.Dtos;
using Common.Dtos.Commands;
using Common.Dtos.Responses;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUser)
        {
            var user = await userService.CreateUserAsync(createUser);

            return Ok(ApiResponse<UserDto>.Ok(user));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            var result = await userService.AuthenticateUserAsync(user);

            return Ok(ApiResponse<AuthResponseDto>.Ok(result));
        }
    }
}
