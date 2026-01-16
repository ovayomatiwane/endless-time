using Common.Dtos;
using Common.Dtos.Commands;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRolesService rolesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await rolesService.GetRolesAsync();

            return Ok(ApiResponse<List<RoleDto>>.Ok(result));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto createRole)
        {
            var role = await rolesService.CreateRoleAsync(createRole);

            return Ok(ApiResponse<RoleDto>.Ok(role));
        }
    }
}
